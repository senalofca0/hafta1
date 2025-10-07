<template>
  <div class="px-4 sm:px-6 lg:px-8">
    <div class="sm:flex sm:items-center">
      <div class="sm:flex-auto">
        <h1 class="text-2xl font-bold text-indigo-900">Ürünler</h1>
        <p class="mt-2 text-sm text-gray-600">
          Sistemde kayıtlı tüm ürünlerin listesi
        </p>
      </div>
      <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
        <router-link
          to="/urun-ekle"
          class="inline-flex items-center justify-center rounded-md border border-transparent bg-indigo-600 px-6 py-3 text-base font-medium text-white shadow-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 transition duration-150 ease-in-out transform hover:scale-105"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
          </svg>
          Yeni Ürün Ekle
        </router-link>
      </div>
    </div>

    <!-- Arama ve Filtreleme -->
    <div class="mt-6 bg-white p-6 rounded-lg shadow-md">
      <div class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <div class="relative">
          <label for="search" class="block text-sm font-semibold text-gray-700 mb-2">Seri No ile Ara</label>
          <div class="relative rounded-md shadow-sm">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
              </svg>
            </div>
            <input
              type="text"
              name="search"
              id="search"
              v-model="searchQuery"
              class="pl-10 focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-lg"
              placeholder="Seri no giriniz..."
            />
          </div>
        </div>

        <div>
          <label for="kalitePuaniFilter" class="block text-sm font-semibold text-gray-700 mb-2">Kalite Puanı Filtresi</label>
          <div class="relative">
            <select
              id="kalitePuaniFilter"
              v-model="kalitePuaniFilter"
              class="block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-lg"
            >
              <option value="">Tümü</option>
              <option value="yuksek">Yüksek (70-100)</option>
              <option value="orta">Orta (40-69)</option>
              <option value="dusuk">Düşük (0-39)</option>
            </select>
            <div class="absolute inset-y-0 right-0 flex items-center px-2 pointer-events-none">
              <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
              </svg>
            </div>
          </div>
        </div>

        <div>
          <label for="durumFilter" class="block text-sm font-semibold text-gray-700 mb-2">Durum Filtresi</label>
          <div class="relative">
            <select
              id="durumFilter"
              v-model="durumFilter"
              class="block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-lg"
            >
              <option value="">Tümü</option>
              <option value="Aktif">Aktif</option>
              <option value="Pasif">Pasif</option>
            </select>
            <div class="absolute inset-y-0 right-0 flex items-center px-2 pointer-events-none">
              <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
              </svg>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="loading" class="mt-8">
      <LoadingSpinner message="Ürünler yükleniyor..." />
    </div>

    <div v-else-if="error" class="mt-8">
      <ErrorMessage :message="error" />
    </div>

    <div v-else-if="successMessage" class="mt-8">
      <SuccessMessage :message="successMessage" />
    </div>

    <div v-else class="mt-8">
      <div class="bg-white rounded-lg shadow overflow-hidden">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-4 text-left text-sm font-semibold text-gray-900">
                Seri No
              </th>
              <th scope="col" class="px-6 py-4 text-left text-sm font-semibold text-gray-900">
                Üretim Tarihi
              </th>
              <th scope="col" class="px-6 py-4 text-left text-sm font-semibold text-gray-900">
                Kalite Puanı
              </th>
              <th scope="col" class="px-6 py-4 text-left text-sm font-semibold text-gray-900">
                Durum
              </th>
              <th scope="col" class="px-6 py-4 text-right text-sm font-semibold text-gray-900">
                İşlemler
              </th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-if="filteredUrunler.length === 0">
              <td colspan="5" class="px-6 py-8 text-center text-sm text-gray-500 bg-gray-50">
                <div class="flex flex-col items-center">
                  <svg class="h-12 w-12 text-gray-400 mb-3" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                  <p class="text-gray-900 font-medium">Ürün Bulunamadı</p>
                  <p class="text-gray-500 mt-1">Arama kriterlerine uygun ürün bulunamadı.</p>
                </div>
              </td>
            </tr>
            <tr v-for="urun in filteredUrunler" :key="urun.id" class="hover:bg-gray-50 transition-colors duration-150">
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                {{ urun.seriNo }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                {{ new Date(urun.uretimTarihi).toLocaleDateString('tr-TR') }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">
                <div class="flex items-center">
                  <div class="w-16 bg-gray-200 rounded-full h-2.5">
                    <div class="h-2.5 rounded-full" :style="{ width: urun.kalitePuani + '%', backgroundColor: getKalitePuaniRenk(urun.kalitePuani) }"></div>
                  </div>
                  <span class="ml-2 text-gray-900">{{ urun.kalitePuani }}</span>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">
                <span :class="[
                  urun.durum === 'Aktif' 
                    ? 'bg-green-100 text-green-800 border border-green-200' 
                    : 'bg-red-100 text-red-800 border border-red-200',
                  'inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium'
                ]">
                  <span :class="[
                    urun.durum === 'Aktif' ? 'bg-green-400' : 'bg-red-400',
                    'flex-shrink-0 h-2 w-2 rounded-full mr-1.5'
                  ]"></span>
                  {{ urun.durum }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <router-link
                  :to="'/urun-duzenle/' + urun.id"
                  class="inline-flex items-center px-3 py-1.5 border border-indigo-100 text-indigo-700 bg-indigo-50 hover:bg-indigo-100 rounded-md mr-2 transition-colors duration-150"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z" />
                    <path fill-rule="evenodd" d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" clip-rule="evenodd" />
                  </svg>
                  Düzenle
                </router-link>
                <button
                  @click="showDeleteConfirmation(urun)"
                  class="inline-flex items-center px-3 py-1.5 border border-red-100 text-red-700 bg-red-50 hover:bg-red-100 rounded-md transition-colors duration-150"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" clip-rule="evenodd" />
                  </svg>
                  Sil
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <DeleteConfirmationModal
      :show="showDeleteModal"
      :title="'Ürün Silme İşlemini Onayla'"
      :message="'Bu ürünü silmek istediğinizden emin misiniz? Bu işlem geri alınamaz.'"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script>
import axios from 'axios'
import LoadingSpinner from '../components/LoadingSpinner.vue'
import ErrorMessage from '../components/ErrorMessage.vue'
import SuccessMessage from '../components/SuccessMessage.vue'
import DeleteConfirmationModal from '../components/DeleteConfirmationModal.vue'

export default {
  name: 'UrunListesi',
  components: {
    LoadingSpinner,
    ErrorMessage,
    SuccessMessage,
    DeleteConfirmationModal
  },
  data() {
    return {
      urunler: [],
      loading: true,
      error: null,
      successMessage: null,
      showDeleteModal: false,
      selectedUrun: null,
      searchQuery: '',
      kalitePuaniFilter: '',
      durumFilter: ''
    }
  },
  computed: {
    filteredUrunler() {
      return this.urunler.filter(urun => {
        const matchesSearch = urun.seriNo.toLowerCase().includes(this.searchQuery.toLowerCase())

        let matchesKalitePuani = true
        if (this.kalitePuaniFilter) {
          const puan = urun.kalitePuani
          switch (this.kalitePuaniFilter) {
            case 'yuksek':
              matchesKalitePuani = puan >= 70 && puan <= 100
              break
            case 'orta':
              matchesKalitePuani = puan >= 40 && puan < 70
              break
            case 'dusuk':
              matchesKalitePuani = puan >= 0 && puan < 40
              break
          }
        }

        const matchesDurum = !this.durumFilter || urun.durum === this.durumFilter

        return matchesSearch && matchesKalitePuani && matchesDurum
      })
    }
  },
  methods: {
    getKalitePuaniRenk(puan) {
      if (puan >= 70) return '#34D399' // Yeşil
      if (puan >= 40) return '#FBBF24' // Sarı
      return '#EF4444' // Kırmızı
    },
    async urunleriGetir() {
      try {
        const response = await axios.get('http://localhost:5176/api/Urun')
        this.urunler = response.data
      } catch (err) {
        this.error = 'Ürünler yüklenirken bir hata oluştu: ' + err.message
      } finally {
        this.loading = false
      }
    },
    showDeleteConfirmation(urun) {
      this.selectedUrun = urun
      this.showDeleteModal = true
    },
    async confirmDelete() {
      if (!this.selectedUrun) return

      this.loading = true
      this.error = null
      this.successMessage = null
      this.showDeleteModal = false

      try {
        await axios.delete(`http://localhost:5176/api/Urun/${this.selectedUrun.id}`)
        this.successMessage = 'Ürün başarıyla silindi'
        await this.urunleriGetir()
      } catch (err) {
        this.error = 'Ürün silinirken bir hata oluştu: ' + err.message
      } finally {
        this.loading = false
        this.selectedUrun = null
      }
    },
    cancelDelete() {
      this.showDeleteModal = false
      this.selectedUrun = null
    }
  },
  created() {
    this.urunleriGetir()
  }
}
</script>