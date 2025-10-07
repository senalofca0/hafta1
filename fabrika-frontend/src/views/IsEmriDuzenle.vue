`<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-2xl mx-auto">
      <h1 class="text-2xl font-bold mb-6">İş Emri Düzenle</h1>

      <LoadingSpinner v-if="loading && !error" />
      <ErrorMessage v-if="error" :message="error" />

      <form v-if="!loading && !error" @submit.prevent="isEmriGuncelle" class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="isEmriKodu">
            İş Emri Kodu
          </label>
          <input
            v-model="isEmri.isEmriKodu"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="isEmriKodu"
            type="text"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="urunID">
            Ürün
          </label>
          <select
            v-model="isEmri.urunID"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="urunID"
            required
          >
            <option value="">Ürün Seçin</option>
            <option v-for="urun in urunler" :key="urun.urunID" :value="urun.urunID">
              {{ urun.seriNo }}
            </option>
          </select>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="baslangicTarihi">
            Başlangıç Tarihi
          </label>
          <input
            v-model="isEmri.baslangicTarihi"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="baslangicTarihi"
            type="datetime-local"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="bitisTarihi">
            Bitiş Tarihi
          </label>
          <input
            v-model="isEmri.bitisTarihi"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="bitisTarihi"
            type="datetime-local"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="durum">
            Durum
          </label>
          <select
            v-model="isEmri.durum"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="durum"
            required
          >
            <option value="Beklemede">Beklemede</option>
            <option value="Devam Ediyor">Devam Ediyor</option>
            <option value="Tamamlandı">Tamamlandı</option>
            <option value="İptal Edildi">İptal Edildi</option>
          </select>
        </div>

        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="aciklama">
            Açıklama
          </label>
          <textarea
            v-model="isEmri.aciklama"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="aciklama"
            rows="3"
          ></textarea>
        </div>

        <FormError v-if="formError" :message="formError" class="mb-4" />

        <div class="flex items-center justify-between">
          <button
            type="submit"
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            :disabled="saving"
          >
            {{ saving ? 'Kaydediliyor...' : 'Kaydet' }}
          </button>
          <router-link
            to="/is-emirleri"
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
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import LoadingSpinner from '../components/LoadingSpinner.vue'
import ErrorMessage from '../components/ErrorMessage.vue'
import FormError from '../components/FormError.vue'

export default {
  name: 'IsEmriDuzenle',
  components: {
    LoadingSpinner,
    ErrorMessage,
    FormError
  },
  setup() {
    const router = useRouter()
    const route = useRoute()
    const loading = ref(true)
    const saving = ref(false)
    const error = ref(null)
    const formError = ref(null)
    const urunler = ref([])
    const isEmri = ref({
      isEmriKodu: '',
      urunID: '',
      baslangicTarihi: '',
      bitisTarihi: '',
      durum: '',
      aciklama: ''
    })

    const formatTarihForInput = (tarih) => {
      return new Date(tarih).toISOString().slice(0, 16)
    }

    const urunleriGetir = async () => {
      try {
        const response = await fetch('http://localhost:5176/api/Urun')
        if (!response.ok) throw new Error('Ürünler yüklenirken bir hata oluştu')
        urunler.value = await response.json()
      } catch (err) {
        error.value = err.message
      }
    }

    const isEmriGetir = async () => {
      try {
        loading.value = true
        error.value = null
        const response = await fetch(`http://localhost:5176/api/IsEmri/${route.params.id}`)
        if (!response.ok) throw new Error('İş emri yüklenirken bir hata oluştu')
        const data = await response.json()
        isEmri.value = {
          ...data,
          baslangicTarihi: formatTarihForInput(data.baslangicTarihi),
          bitisTarihi: formatTarihForInput(data.bitisTarihi)
        }
      } catch (err) {
        error.value = err.message
      } finally {
        loading.value = false
      }
    }

    const isEmriGuncelle = async () => {
      if (new Date(isEmri.value.baslangicTarihi) >= new Date(isEmri.value.bitisTarihi)) {
        formError.value = 'Başlangıç tarihi, bitiş tarihinden önce olmalıdır.'
        return
      }

      try {
        saving.value = true
        formError.value = null
        const response = await fetch(`http://localhost:5176/api/IsEmri/${route.params.id}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(isEmri.value)
        })

        if (!response.ok) throw new Error('İş emri güncellenirken bir hata oluştu')
        
        router.push('/is-emirleri')
      } catch (err) {
        formError.value = err.message
      } finally {
        saving.value = false
      }
    }

    onMounted(async () => {
      await Promise.all([urunleriGetir(), isEmriGetir()])
    })

    return {
      isEmri,
      urunler,
      loading,
      saving,
      error,
      formError,
      isEmriGuncelle
    }
  }
}
</script>`