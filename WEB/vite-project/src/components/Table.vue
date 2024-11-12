<template>
  <n-data-table
    :columns="columns"
    :data="data"
  >

  </n-data-table>
</template>
<script lang="ts" setup>
import {NDataTable} from 'naive-ui'


const columns: Array<any> = [
  {
    title: 'Город',
    key: 'City',
    rowSpan: (dataRow: any) => dataRow.rowSpan ?? 1,
    colSpan: (dataRow: any) => dataRow.colSpan ?? 1
  },
  {
    title: 'Имя',
    key: 'Name',
    rowSpan: (dataRow: any) => dataRow.rowSpan ?? 1,
    colSpan: (dataRow: any) => dataRow.colSpan ?? 1
  },
  {
    title: 'Caffe Espresso',
    children: [
      {
        title: 'Сумма',
        key: 'cafe1.sum'
      },
      {
        title: 'Количество'
      },
      {
        title: 'Среднее'
      },
      {
        title: ''
      }
    ]
  },
  {
    title: 'Espresso con Panna',
    children: [
      {
        title: 'Сумма'
      },
      {
        title: 'Количество'
      },
      {
        title: 'Среднее'
      }
    ]
  },
  {
    title: 'White Chocolate Mocha',
    children: [
      {
        title: 'Сумма'
      },
      {
        title: 'Количество'
      },
      {
        title: 'Среднее'
      }
    ]
  }
]


const data: Array<any> = [
  {
    City: 'New York',
    Name: 'Total',
    rowSpan: 3,
    cafe1: {
      sum: 1000
    }
  },
  {
    Name: 'Имя'
  },
  {
    Name: 'Имя'
  }
] 

</script>
<style scoped>
</style>