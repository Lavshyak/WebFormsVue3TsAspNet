<script setup lang="ts">
import {computed, reactive, ref} from 'vue';
import {useFormsSearching} from "@/hooks/useFormsSearching.ts";

const formatJson = (val:any) => JSON.stringify(val, null, 2);

const searchingForm = reactive({
  formType: '',
  maxRowsOnPage: 5
})

const formTypeHint = computed<string | null>(() => {
  if (searchingForm.formType.trim().length !== searchingForm.formType.length) {
    return '(has leading/trailing spaces)'
  }
  return null;
})

const formsSearching = useFormsSearching()

function search() {
  formsSearching.newSearch({
    formType: searchingForm.formType,
    maxRowsOnPage: searchingForm.maxRowsOnPage,
    pageNumber: 1
  })
}

search()

function nextPage() {
  formsSearching.tryMoveToPage(formsSearching.currentPageNumber.value + 1)
}

function prevPage() {
  formsSearching.tryMoveToPage(formsSearching.currentPageNumber.value - 1)
}

</script>

<template>
  <div class="component-container">
    <div class="form-type-container">
      Form type:
      <input v-model="searchingForm.formType"/>
      <div v-if="formTypeHint !== null" style="color: #000000EE">
        - {{ formTypeHint }}
      </div>
    </div>
    <button :disabled="formsSearching.isFetching.value" @click="search" type="submit">
      Search
    </button>

    <div class="paging-buttons-container">
      <button
          :style="{visibility: formsSearching.currentPageNumber.value > 1 ? 'visible' : 'hidden'}"
          :disabled="formsSearching.isFetching.value"
          @click="prevPage">
        <
      </button>
      <div>
        {{ formsSearching.currentPageNumber.value }}
      </div>
      <button
          :style="{visibility: formsSearching.currentPageForms.value.length ===
          searchingForm.maxRowsOnPage &&
          formsSearching.nextPageIsEmpty.value === false
          ? 'visible' : 'hidden'}"
          :disabled="formsSearching.isFetching.value"
          @click="nextPage">
        >
      </button>
    </div>

    <table>
      <thead>
      <tr>
        <th class="th-guid">GUID</th>
        <th class="th-date">Submitted at</th>
        <th>Form content</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="row in formsSearching.currentPageForms.value" :key="row.guid">
        <td class="td-guid">{{ row.guid }}</td>
        <td class="td-date">{{ row.createdAt }}</td>
        <td>
          <pre>{{ formatJson(row.formJson) }}</pre>
        </td>
      </tr>
      </tbody>
    </table>
    <div v-if="formsSearching.currentPageForms.value.length == 0" class="nodata">
      no data
    </div>
  </div>

</template>

<style scoped>
.component-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  align-items: start;
}

.form-type-container {
  display: flex;
  flex-direction: row;
  gap: 0.5rem
}

.paging-buttons-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
  width: 100%;
  gap: 0.5rem;
}

table {
  border-collapse: collapse;
  width: 100%;
}

th, td {
  border: 1px solid #00000033;
  padding: 8px;
}

.th-guid, .th-date {
  width: 10rem;
}

.td-guid, .td-date {
  width: 5rem;
}

pre {
  margin: 0;
  white-space: pre-wrap;
}

.nodata {
  width: 100%;
  display: flex;
  justify-content: center;
  font-size: 1.5rem;
}


</style>