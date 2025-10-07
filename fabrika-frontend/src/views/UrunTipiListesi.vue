`<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Ürün Tipleri</h1>
      <router-link
        to="/urun-tipi-ekle"
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
      >
        Yeni Ürün Tipi Ekle
      </router-link>
    </div>

    <LoadingSpinner v-if="loading" />
    <ErrorMessage v-if="error" :message="error" />

    <div v-if="!loading && !error" class="bg-white shadow-md rounded-lg overflow-hidden">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ürün Adı</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ürün Kodu</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Min. Kalite Skoru</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Max. Kalite Skoru</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Durum</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">İşlemler</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="urunTipi in urunTipleri" :key="urunTipi.urunTipiID">
            <td class="px-6 py-4 whitespace-nowrap">{{ urunTipi.urunAdi }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ urunTipi.urunKodu }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ urunTipi.minKaliteSkoru }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ urunTipi.maxKaliteSkoru }}</td>
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                :class="[
                  'px-2 inline-flex text-xs leading-5 font-semibold rounded-full',
                  urunTipi.aktifMi ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                ]"
              >
                {{ urunTipi.aktifMi ? 'Aktif' : 'Pasif' }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
              <router-link
                :to="'/urun-tipi-duzenle/' + urunTipi.urunTipiID"
                class="text-blue-600 hover:text-blue-900 mr-4"
              >
                Düzenle
              </router-link>
              <button
                @click="silmeOnayiGoster(urunTipi)"
                class="text-red-600 hover:text-red-900"
              >
                Sil
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <DeleteConfirmationModal
      v-if="showDeleteModal"
      :item-name="silinecekUrunTipi?.urunAdi"
      @confirm="urunTipiSil"
      @cancel="silmeIptal"
    />
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import LoadingSpinner from '../components/LoadingSpinner.vue'
import ErrorMessage from '../components/ErrorMessage.vue'
import DeleteConfirmationModal from '../components/DeleteConfirmationModal.vue'

export default {
  name: 'UrunTipiListesi',
  components: {
    LoadingSpinner,
    ErrorMessage,
    DeleteConfirmationModal
  },
  setup() {
    const urunTipleri = ref([])
    const loading = ref(true)
    const error = ref(null)
    const showDeleteModal = ref(false)
    const silinecekUrunTipi = ref(null)

    const urunTipleriGetir = async () => {
      try {
        loading.value = true
        error.value = null
        const response = await fetch('http://localhost:5176/api/UrunTipi')
        if (!response.ok) throw new Error('Ürün tipleri yüklenirken bir hata oluştu')
        urunTipleri.value = await response.json()
      } catch (err) {
        error.value = err.message
      } finally {
        loading.value = false
      }
    }

    const silmeOnayiGoster = (urunTipi) => {
      silinecekUrunTipi.value = urunTipi
      showDeleteModal.value = true
    }

    const urunTipiSil = async () => {
      try {
        const response = await fetch(`http://localhost:5176/api/UrunTipi/${silinecekUrunTipi.value.urunTipiID}`, {
          method: 'DELETE'
        })
        if (!response.ok) throw new Error('Ürün tipi silinirken bir hata oluştu')
        await urunTipleriGetir()
        showDeleteModal.value = false
      } catch (err) {
        error.value = err.message
      }
    }

    const silmeIptal = () => {
      showDeleteModal.value = false
      silinecekUrunTipi.value = null
    }

    onMounted(urunTipleriGetir)

    return {
      urunTipleri,
      loading,
      error,
      showDeleteModal,
      silinecekUrunTipi,
      silmeOnayiGoster,
      urunTipiSil,
      silmeIptal
    }
  }
}
</script>`