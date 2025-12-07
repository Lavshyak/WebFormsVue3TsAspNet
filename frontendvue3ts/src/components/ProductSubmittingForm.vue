<script setup lang="ts">
import {computed, reactive, ref} from "vue";
import useVuelidate from "@vuelidate/core";
import {helpers, required} from "@vuelidate/validators";
import {type JsonNode, postFormsStoreClient} from "@/gen";
import type {AxiosError} from "axios";

const productCategories = ['furniture', 'car', 'plane'].map(s => ({value: s, label: s[0]!.toUpperCase() + s.slice(1)}))

const colors = ['white', 'black', 'green'].map(s => ({value: s, label: s[0]!.toUpperCase() + s.slice(1)}))

const validationRules = computed(() => ({
  title: {
    required,
    noSpaces: helpers.withMessage('No leading/trailing spaces', (t: string) => t.trim().length === t.length),
  },
  category: {
    required,
    notEmpty: helpers.withMessage('Undefined', (x) => x !== ''),
  },
  releaseDate: {
    required,
    notEmpty: helpers.withMessage('Undefined', (x) => x !== ''),
    isDate: helpers.withMessage('Is not a Date', (dStr: string) => !isNaN(Date.parse(dStr))),
    isValidYear: helpers.withMessage('Year must be less than 2100', (dStr: string) => new Date(dStr).getFullYear() < 2100),
  },
  color: {
    required,
    notEmpty: helpers.withMessage('Undefined', (x) => x !== ''),
  },
  isLimitedEdition: {
    required,
    isBoolean: helpers.withMessage('Boolean', (x) => x === true || x === false),
  }
}))


const form = reactive<{
  title: string,
  category: string,
  releaseDate: string,
  color: string,
  isLimitedEdition: boolean
}>(
    {
      title: '',
      category: '',
      releaseDate: '',
      color: '',
      isLimitedEdition: false,
    })

const $v = useVuelidate(validationRules, form)

const isSubmittingInRequestingState = ref(false)
const submittingResult = ref<string|null>(null)

function submit() {
  if ($v.value.$invalid) {
    $v.value.$touch()
    return
  }

  const objectToPost = {
    formType: 'productReleases',
    formData: form
  }

  isSubmittingInRequestingState.value = true
  postFormsStoreClient(objectToPost as JsonNode)
      .then(respCfg => {
        if(respCfg.status === 200){
          submittingResult.value = `OK, requested at: ${new Date()}, form GUID: ${respCfg.data}`;
        }
        else {
          submittingResult.value = `ERROR, requested at: ${new Date()}, status: ${respCfg.statusText}`;
        }
      })
      .catch(reason => {
        submittingResult.value = `ERROR, requested at: ${new Date()}, reason: ${reason.message !== undefined ? reason.message : reason}`;
      })
      .finally(()=>isSubmittingInRequestingState.value = false)
}

</script>

<template>

    <div class="form-container">
      FormType: productReleases
      <br/>
      <br/>
      Product title:
      <div class="container-left-input-right-error">
        <input
            type="text"
            v-model="$v.title.$model"
            placeholder="Product title"
        />
        <div v-if="$v.title.$dirty && $v.title.$invalid" class="error">
          - {{ $v.title.$errors[0]?.$message }}
        </div>
      </div>


      Category:
      <div class="container-left-input-right-error">
        <select v-model="form.category">
          <option disabled value=''>Choose product category</option>
          <option
              v-for="opt in productCategories"
              :key="opt.value"
              :value="opt.value"
          >
            {{ opt.label }}
          </option>
        </select>
        <div v-if="$v.category.$dirty && $v.category.$invalid" class="error">
          - {{ $v.category.$errors[0]?.$message }}
        </div>
      </div>

      Release date:
      <div class="container-left-input-right-error">
        <input
            type="date"
            v-model="$v.releaseDate.$model"
        />
        <div v-if="$v.releaseDate.$dirty && $v.releaseDate.$invalid" class="error">
          - {{ $v.releaseDate.$errors[0]?.$message }}
        </div>
      </div>


      Color:
      <div class="container-left-input-right-error">
        <div class="stack-horizontal">
          <label v-for="opt in colors" :key="opt.value">
            <input
                type="radio"
                :value="opt.value"
                v-model="$v.color.$model"
            />
            {{ opt.label }}
          </label>
        </div>
        <div v-if="$v.color.$dirty && $v.color.$invalid" class="error">
          - {{ $v.color.$errors[0]?.$message }}
        </div>

      </div>


      <div class="container-left-input-right-error">
        <label>
          <input type="checkbox" v-model="$v.isLimitedEdition.$model"/>
          Is limited edition?
        </label>
      </div>

      <button :disabled="isSubmittingInRequestingState" class="submit-btn" @click="submit">
        Submit
      </button>

      <div v-if="submittingResult !== null">
        {{submittingResult}}
      </div>

    </div>

</template>

<style scoped>

.form-container {
  display: flex;
  flex-direction: column;
  justify-content: start;
  width: min(40rem, 100%);
}

.container-left-input-right-error {
  display: flex;
  flex-direction: row;
  margin-bottom: 0.5rem;

}

.container-left-input-right-error > *:nth-child(1) {
  width: 58%;
}

.container-left-input-right-error > *:nth-child(2) {
  width: 38%;
}

.error {
  color: red;
  padding-left: 1rem;
}

.submit-btn {
  width: 30%;
}
</style>