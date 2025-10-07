`<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">İş Emirleri</h1>
      <router-link
        to="/is-emri-ekle"
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
      >
        Yeni İş Emri Ekle
      </router-link>
    </div>

    <LoadingSpinner v-if="loading" />
    <ErrorMessage v-if="error" :message="error" />

    <div v-if="!loading && !error" class="bg-white shadow-md rounded-lg overflow-hidden">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">İş Emri Kodu</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ürün</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Başlangıç Tarihi</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Bitiş Tarihi</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Durum</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">İşlemler</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="isEmri in isEmirleri" :key="isEmri.isEmriID">
            <td class="px-6 py-4 whitespace-nowrap">{{ isEmri.isEmriKodu }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ isEmri.urun?.seriNo }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ formatTarih(isEmri.baslangicTarihi) }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ formatTarih(isEmri.bitisTarihi) }}</td>
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                :class="[
                  'px-2 inline-flex text-xs leading-5 font-semibold rounded-full',
                  getDurumClass(isEmri.durum)
                ]"
              >
                {{ isEmri.durum }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
              <router-link
                :to="'/is-emri-duzenle/' + isEmri.isEmriID"
                class="text-blue-600 hover:text-blue-900 mr-4"
              >
                Düzenle
              </router-link>
              <button
                @click="silmeOnayiGoster(isEmri)"
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
      :item-name="silinecekIsEmri?.isEmriKodu"
      @confirm="isEmriSil"
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
  name: 'IsEmriListesi',
  components: {
    LoadingSpinner,
    ErrorMessage,
    DeleteConfirmationModal
  },
  setup() {
    const isEmirleri = ref([])
    const loading = ref(true)
    const error = ref(null)
    const showDeleteModal = ref(false)
    const silinecekIsEmri = ref(null)

    const formatTarih = (tarih) => {
      return new Date(tarih).toLocaleDateString('tr-TR')
    }

    const getDurumClass = (durum) => {
      const classes = {
        'Beklemede': 'bg-yellow-100 text-yellow-800',
        'Devam Ediyor': 'bg-blue-100 text-blue-800',
        'Tamamlandı': 'bg-green-100 text-green-800',
        'İptal Edildi': 'bg-red-100 text-red-800'
      }
      return classes[durum] || 'bg-gray-100 text-gray-800'
    }

    const isEmirleriGetir = async () => {
      try {
        loading.value = true
        error.value = null
        const response = await fetch('http://localhost:5176/api/IsEmri')
        if (!response.ok) throw new Error('İş emirleri yüklenirken bir hata oluştu')
        isEmirleri.value = await response.json()
      } catch (err) {
        error.value = err.message
      } finally {
        loading.value = false
      }
    }

    const silmeOnayiGoster = (isEmri) => {
      silinecekIsEmri.value = isEmri
      showDeleteModal.value = true
    }

    const isEmriSil = async () => {
      try {
        const response = await fetch(`http://localhost:5176/api/IsEmri/${silinecekIsEmri.value.isEmriID}`, {
          method: 'DELETE'
        })
        if (!response.ok) throw new Error('İş emri silinirken bir hata oluştu')
        await isEmirleriGetir()
        showDeleteModal.value = false
      } catch (err) {
        error.value = err.message
      }
    }

    const silmeIptal = () => {
      showDeleteModal.value = false
      silinecekIsEmri.value = null
    }

    onMounted(isEmirleriGetir)

    return {
      isEmirleri,
      loading,
      error,
      showDeleteModal,
      silinecekIsEmri,
      formatTarih,
      getDurumClass,
      silmeOnayiGoster,
      isEmriSil,
      silmeIptal
    }
  }
}
</script>`