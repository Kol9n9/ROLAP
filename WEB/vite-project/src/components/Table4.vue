<template>
    <table>
    <thead>
        <tr>
            <th rowspan="2" class="row-header">Город</th>
            <th rowspan="2" class="row-header">Имя</th>
            <th colspan="3">Caffe Espresso</th>
            <th colspan="3">Espresso con Panna</th>
            <th colspan="3">White Chocolate Mocha</th>
        </tr>
        <tr>
            <th>Сумма</th>
            <th>Количество</th>
            <th>Среднее</th>
            <th>Сумма</th>
            <th>Количество</th>
            <th>Среднее</th>
            <th>Сумма</th>
            <th>Количество</th>
            <th>Среднее</th>
        </tr>
    </thead>
    <tbody>
        <!-- Город: New York -->
        <tr>
            <td rowspan="2" class="row-header">New York</td>
            <td class="row-header">Antoni</td>
            <td>$18.00</td><td>6</td><td>$3.00</td>
            <td>$3.25</td><td>1</td><td>$3.25</td>
            <td>$3.60</td><td>1</td><td>$3.60</td>
        </tr>
        <tr>
            <td class="row-header">Davolio</td>
            <td>$12.00</td><td>4</td><td>$3.00</td>
            <td>$3.25</td><td>1</td><td>$3.25</td>
            <td></td><td></td><td></td>
        </tr>
        <!-- Город: Seattle -->
        <tr>
            <td rowspan="2" class="row-header">Seattle</td>
            <td class="row-header">Fuller</td>
            <td>$12.00</td><td>4</td><td>$3.00</td>
            <td>$6.50</td><td>2</td><td>$3.25</td>
            <td></td><td></td><td></td>
        </tr>
        <tr>
            <td class="row-header">Murphy</td>
            <td>$3.00</td><td>1</td><td>$3.00</td>
            <td></td><td></td><td></td>
            <td></td><td></td><td></td>
        </tr>
        <!-- Город: Chicago с агрегацией -->
        <tr>
            <td rowspan="4" class="row-header">Chicago</td>
            <td class="row-header">Andrew</td>
            <td>$21.00</td><td>7</td><td>$3.00</td>
            <td>$32.50</td><td>10</td><td>$3.25</td>
            <td>$7.20</td><td>2</td><td>$3.60</td>
        </tr>
        <tr>
            <td class="row-header">Nancy</td>
            <td>$21.00</td><td>7</td><td>$3.00</td>
            <td>$16.25</td><td>5</td><td>$3.25</td>
            <td>$14.40</td><td>4</td><td>$3.60</td>
        </tr>
        <tr>
            <td class="row-header">Peter</td>
            <td>$21.00</td><td>7</td><td>$3.00</td>
            <td>$13.00</td><td>4</td><td>$3.25</td>
            <td>$14.40</td><td>4</td><td>$3.60</td>
        </tr>
        <tr class="aggregation">
            <td colspan="1" class="row-header">Итого</td>
            <td>$63.00</td><td>21</td><td>$3.00</td>
            <td>$61.75</td><td>19</td><td>$3.25</td>
            <td>$36.00</td><td>10</td><td>$3.60</td>
        </tr>
        <!-- Город: Los Angeles -->
        <tr>
            <td rowspan="2" class="row-header">Los Angeles</td>
            <td class="row-header">Shelley</td>
            <td>$15.00</td><td>5</td><td>$3.00</td>
            <td>$6.50</td><td>2</td><td>$3.25</td>
            <td>$18.00</td><td>5</td><td>$3.60</td>
        </tr>
        <tr>
            <td class="row-header">Regina</td>
            <td>$15.00</td><td>5</td><td>$3.00</td>
            <td>$6.50</td><td>2</td><td>$3.25</td>
            <td>$18.00</td><td>5</td><td>$3.60</td>
        </tr>
        <!-- Город: Miami -->
        <tr>
            <td rowspan="2" class="row-header">Miami</td>
            <td class="row-header">Yoshi</td>
            <td>$15.00</td><td>5</td><td>$3.00</td>
            <td>$6.50</td><td>2</td><td>$3.25</td>
            <td>$18.00</td><td>5</td><td>$3.60</td>
        </tr>
        <tr>
            <td class="row-header">Ian</td>
            <td>$9.00</td><td>3</td><td>$3.00</td>
            <td>$19.50</td><td>6</td><td>$3.25</td>
            <td>$32.40</td><td>9</td><td>$3.60</td>
        </tr>
    </tbody>
</table>
</template>
<script lang="ts" setup></script>
<style>
 table {
            width: 100%;
            border-collapse: collapse;
        }
        th, td {
            padding: 8px;
            text-align: center;
            border: 1px solid #ddd;
        }
        th {
            background-color: #f4f4f4;
        }
        .row-header {
            font-weight: bold;
            text-align: left;
        }
        .aggregation {
            font-weight: bold;
            background-color: #e8e8e8;
        }
</style>