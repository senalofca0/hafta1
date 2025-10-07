`<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-2xl mx-auto">
      <h1 class="text-2xl font-bold mb-6">Yeni Ürün Tipi Ekle</h1>

      <form @submit.prevent="urunTipiEkle" class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="urunAdi">
            Ürün Adı
          </label>
          <input
            v-model="urunTipi.urunAdi"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="urunAdi"
            type="text"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="urunKodu">
            Ürün Kodu
          </label>
          <input
            v-model="urunTipi.urunKodu"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="urunKodu"
            type="text"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="aciklama">
            Açıklama
          </label>
          <textarea
            v-model="urunTipi.aciklama"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="aciklama"
            rows="3"
          ></textarea>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="minKaliteSkoru">
            Minimum Kalite Skoru
          </label>
          <input
            v-model.number="urunTipi.minKaliteSkoru"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="minKaliteSkoru"
            type="number"
            step="0.1"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="maxKaliteSkoru">
            Maksimum Kalite Skoru
          </label>
          <input
            v-model.number="urunTipi.maxKaliteSkoru"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="maxKaliteSkoru"
            type="number"
            step="0.1"
            required
          >
        </div>

        <div class="mb-6">
          <label class="flex items-center">
            <input
              v-model="urunTipi.aktifMi"
              type="checkbox"
              class="form-checkbox h-5 w-5 text-blue-600"
            >
            <span class="ml-2 text-gray-700">Aktif</span>
          </label>
        </div>

        <FormError v-if="error" :message="error" class="mb-4" />

        <div class="flex items-center justify-between">
          <button
            type="submit"
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            :disabled="loading"
          >
            {{ loading ? 'Kaydediliyor...' : 'Kaydet' }}
          </button>
          <router-link
            to="/urun-tipleri"
            class="text-blue-500 hover:text-blue-800"
          >
            İptal
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import FormError from '../components/FormError.vue'

export default {
  name: 'UrunTipiEkle',
  components: {
    FormError
  },
  setup() {
    const router = useRouter()
    const loading = ref(false)
    const error = ref(null)
    const urunTipi = ref({
      urunAdi: '',
      urunKodu: '',
      aciklama: '',
      minKaliteSkoru: 0,
      maxKaliteSkoru: 100,
      aktifMi: true
    })

    const urunTipiEkle = async () => {
      if (urunTipi.value.minKaliteSkoru >= urunTipi.value.maxKaliteSkoru) {
        error.value = 'Minimum kalite skoru, maksimum kalite skorundan küçük olmalıdır.'
        return
      }

      try {
        loading.value = true
        error.value = null
        const response = await fetch('http://localhost:5176/api/UrunTipi', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(urunTipi.value)
        })

        if (!response.ok) throw new Error('Ürün tipi eklenirken bir hata oluştu')
        
        router.push('/urun-tipleri')
      } catch (err) {
        error.value = err.message
      } finally {
        loading.value = false
      }
    }

    return {
      urunTipi,
      loading,
      error,
      urunTipiEkle
    }
  }
}
</script>`