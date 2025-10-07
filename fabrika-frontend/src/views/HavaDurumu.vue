<template>
  <div class="container mx-auto p-4">
    <h1 class="text-3xl font-bold mb-6">Hava Durumu</h1>
    
    <div class="mb-4">
      <input 
        v-model="sehir" 
        type="text" 
        placeholder="Şehir adı girin..." 
        class="p-2 border rounded-lg mr-2"
        @keyup.enter="havaDurumuGetir"
      >
      <button 
        @click="havaDurumuGetir" 
        class="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
      >
        Ara
      </button>
    </div>

    <div v-if="yukleniyor" class="text-center">
      <p>Yükleniyor...</p>
    </div>

    <div v-else-if="hata" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-4">
      {{ hata }}
    </div>

    <div v-else-if="havaDurumu" class="bg-white rounded-lg shadow-lg p-6">
      <div class="flex items-center justify-between mb-4">
        <h2 class="text-2xl font-semibold">{{ havaDurumu.name }}, {{ havaDurumu.sys.country }}</h2>
        <p class="text-4xl font-bold">{{ Math.round(havaDurumu.main.temp) }}°C</p>
      </div>
      
      <div class="grid grid-cols-2 gap-4">
        <div class="bg-gray-50 p-4 rounded-lg">
          <p class="text-gray-600">Hissedilen</p>
          <p class="text-xl font-semibold">{{ Math.round(havaDurumu.main.feels_like) }}°C</p>
        </div>
        <div class="bg-gray-50 p-4 rounded-lg">
          <p class="text-gray-600">Nem</p>
          <p class="text-xl font-semibold">{{ havaDurumu.main.humidity }}%</p>
        </div>
        <div class="bg-gray-50 p-4 rounded-lg">
          <p class="text-gray-600">Rüzgar Hızı</p>
          <p class="text-xl font-semibold">{{ havaDurumu.wind.speed }} m/s</p>
        </div>
        <div class="bg-gray-50 p-4 rounded-lg">
          <p class="text-gray-600">Durum</p>
          <p class="text-xl font-semibold capitalize">{{ havaDurumu.weather[0].description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const API_KEY = import.meta.env.VITE_WEATHER_API_KEY
const sehir = ref('')
const havaDurumu = ref(null)
const yukleniyor = ref(false)
const hata = ref(null)

async function havaDurumuGetir() {
  if (!sehir.value) {
    hata.value = 'Lütfen bir şehir adı girin.'
    return
  }

  try {
    yukleniyor.value = true
    hata.value = null
    const response = await fetch(
      `https://api.openweathermap.org/data/2.5/weather?q=${sehir.value}&appid=${API_KEY}&units=metric&lang=tr`
    )
    
    if (!response.ok) {
      throw new Error('Şehir bulunamadı')
    }

    havaDurumu.value = await response.json()
  } catch (error) {
    hata.value = error.message
    havaDurumu.value = null
  } finally {
    yukleniyor.value = false
  }
}
</script>