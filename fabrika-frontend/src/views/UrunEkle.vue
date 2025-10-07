<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 py-8">
    <div class="max-w-3xl mx-auto">
      <!-- Başlık Kartı -->
      <div class="bg-white rounded-lg shadow-sm mb-6 p-6">
        <div class="flex items-center space-x-4">
          <div class="p-2 bg-indigo-100 rounded-lg">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-indigo-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </div>
          <h2 class="text-2xl font-bold text-gray-900">Yeni Ürün Ekle</h2>
        </div>
      </div>

      <!-- Ana Form Kartı -->
      <div class="bg-white rounded-lg shadow-sm">
        <div class="p-6">
          <div v-if="loading" class="flex items-center justify-center py-12">
            <LoadingSpinner message="Ürün ekleniyor..." />
          </div>
          
          <div v-else-if="error" class="py-6">
            <ErrorMessage :message="error" />
          </div>
          
          <div v-else-if="successMessage" class="py-6">
            <SuccessMessage :message="successMessage" />
          </div>
          
          <form v-else @submit.prevent="urunEkle" class="space-y-6">
            <!-- Seri No -->
            <div class="form-group">
              <label for="seriNo" class="block text-sm font-semibold text-gray-700 mb-2">
                Seri No
              </label>
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M4 4a2 2 0 012-2h8a2 2 0 012 2v12a1 1 0 01-1 1H5a1 1 0 01-1-1V4zm3 1h6v4H7V5zm6 6H7v2h6v-2z" clip-rule="evenodd" />
                  </svg>
                </div>
                <input
                  type="text"
                  id="seriNo"
                  v-model="urun.seriNo"
                  required
                  @blur="validateSeriNo"
                  :class="{'border-red-300 ring-red-500': errors.seriNo}"
                  class="pl-10 block w-full rounded-lg border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm transition duration-150"
                  placeholder="ABC123"
                />
              </div>
              <FormError :error="errors.seriNo" />
            </div>

            <!-- Üretim Tarihi -->
            <div class="form-group">
              <label for="uretimTarihi" class="block text-sm font-semibold text-gray-700 mb-2">
                Üretim Tarihi
              </label>
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd" />
                  </svg>
                </div>
                <input
                  type="date"
                  id="uretimTarihi"
                  v-model="urun.uretimTarihi"
                  required
                  @blur="validateUretimTarihi"
                  :class="{'border-red-300 ring-red-500': errors.uretimTarihi}"
                  class="pl-10 block w-full rounded-lg border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm transition duration-150"
                />
              </div>
              <FormError :error="errors.uretimTarihi" />
            </div>

            <!-- Kalite Puanı -->
            <div class="form-group">
              <label for="kalitePuani" class="block text-sm font-semibold text-gray-700 mb-2">
                Kalite Puanı
              </label>
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                  </svg>
                </div>
                <input
                  type="number"
                  id="kalitePuani"
                  v-model="urun.kalitePuani"
                  min="0"
                  max="100"
                  required
                  @blur="validateKalitePuani"
                  :class="{'border-red-300 ring-red-500': errors.kalitePuani}"
                  class="pl-10 block w-full rounded-lg border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm transition duration-150"
                  placeholder="85"
                />
              </div>
              <FormError :error="errors.kalitePuani" />
            </div>

            <!-- Durum -->
            <div class="form-group">
              <label for="durum" class="block text-sm font-semibold text-gray-700 mb-2">
                Durum
              </label>
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M10 2a8 8 0 100 16 8 8 0 000-16zm0 14a6 6 0 110-12 6 6 0 010 12z" clip-rule="evenodd" />
                  </svg>
                </div>
                <select
                  id="durum"
                  v-model="urun.durum"
                  required
                  @blur="validateDurum"
                  :class="{'border-red-300 ring-red-500': errors.durum}"
                  class="pl-10 block w-full rounded-lg border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm transition duration-150"
                >
                  <option value="Aktif">Aktif</option>
                  <option value="Pasif">Pasif</option>
                </select>
              </div>
              <FormError :error="errors.durum" />
            </div>

            <!-- Butonlar -->
            <div class="flex justify-end space-x-3 pt-6">
              <router-link
                to="/"
                class="inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-lg text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition duration-150"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2 -ml-1" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
                </svg>
                İptal
              </router-link>
              <button
                type="submit"
                :disabled="loading || hasErrors"
                class="inline-flex items-center px-4 py-2 border border-transparent shadow-sm text-sm font-medium rounded-lg text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed transition duration-150"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2 -ml-1" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
                </svg>
                Kaydet
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import LoadingSpinner from '../components/LoadingSpinner.vue'
import ErrorMessage from '../components/ErrorMessage.vue'
import SuccessMessage from '../components/SuccessMessage.vue'
import FormError from '../components/FormError.vue'

export default {
  name: 'UrunEkle',
  components: {
    LoadingSpinner,
    ErrorMessage,
    SuccessMessage,
    FormError
  },
  data() {
    return {
      urun: {
        seriNo: '',
        uretimTarihi: '',
        kalitePuani: '',
        durum: 'Aktif'
      },
      errors: {
        seriNo: '',
        uretimTarihi: '',
        kalitePuani: '',
        durum: ''
      },
      loading: false,
      error: null,
      successMessage: null
    }
  },
  computed: {
    hasErrors() {
      return Object.values(this.errors).some(error => error !== '') ||
        !this.urun.seriNo ||
        !this.urun.uretimTarihi ||
        !this.urun.kalitePuani ||
        !this.urun.durum
    }
  },
  methods: {
    validateSeriNo() {
      if (!this.urun.seriNo) {
        this.errors.seriNo = 'Seri no boş bırakılamaz'
      } else if (this.urun.seriNo.length < 3) {
        this.errors.seriNo = 'Seri no en az 3 karakter olmalıdır'
      } else {
        this.errors.seriNo = ''
      }
    },
    validateUretimTarihi() {
      if (!this.urun.uretimTarihi) {
        this.errors.uretimTarihi = 'Üretim tarihi boş bırakılamaz'
      } else {
        const selectedDate = new Date(this.urun.uretimTarihi)
        const today = new Date()
        if (selectedDate > today) {
          this.errors.uretimTarihi = 'Üretim tarihi gelecek bir tarih olamaz'
        } else {
          this.errors.uretimTarihi = ''
        }
      }
    },
    validateKalitePuani() {
      const puan = Number(this.urun.kalitePuani)
      if (this.urun.kalitePuani === '') {
        this.errors.kalitePuani = 'Kalite puanı boş bırakılamaz'
      } else if (isNaN(puan)) {
        this.errors.kalitePuani = 'Kalite puanı sayısal bir değer olmalıdır'
      } else if (puan < 0 || puan > 100) {
        this.errors.kalitePuani = 'Kalite puanı 0-100 arasında olmalıdır'
      } else {
        this.errors.kalitePuani = ''
      }
    },
    validateDurum() {
      if (!this.urun.durum) {
        this.errors.durum = 'Durum seçilmelidir'
      } else if (!['Aktif', 'Pasif'].includes(this.urun.durum)) {
        this.errors.durum = 'Geçersiz durum değeri'
      } else {
        this.errors.durum = ''
      }
    },
    async urunEkle() {
      if (this.hasErrors) return

      this.loading = true
      this.error = null
      this.successMessage = null

      try {
        await axios.post('http://localhost:5176/api/Urun', {
          seriNo: this.urun.seriNo,
          uretimTarihi: this.urun.uretimTarihi,
          kalitePuani: Number(this.urun.kalitePuani),
          durum: this.urun.durum
        })

        this.successMessage = 'Ürün başarıyla eklendi'
        this.urun = {
          seriNo: '',
          uretimTarihi: '',
          kalitePuani: '',
          durum: 'Aktif'
        }

        setTimeout(() => {
          this.$router.push('/')
        }, 2000)
      } catch (err) {
        this.error = err.response?.data?.message || 'Bir hata oluştu'
      } finally {
        this.loading = false
      }
    }
  }
}
</script>

<style scoped>
.form-group {
  position: relative;
}

.form-group:hover .absolute svg {
  color: #6366f1;
}
</style>