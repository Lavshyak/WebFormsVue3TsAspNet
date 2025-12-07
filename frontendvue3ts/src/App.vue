<script setup lang="ts">
import ProductSubmittingForm from "@/components/ProductSubmittingForm.vue";
import FormsSearchingWithTable from "@/components/FormsSearchingWithTable.vue";
</script>

<template>
  <header>
  </header>

  <main>
    <div class="page-body">
      <ProductSubmittingForm/>
      <FormsSearchingWithTable/>
    </div>
  </main>
</template>

<style scoped>
.page-body {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
}
</style>
