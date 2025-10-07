<template>
  <div class="bg-white shadow sm:rounded-lg">
    <div class="px-4 py-5 sm:p-6">
      <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4">
        Ürün Düzenle
      </h3>
      <div v-if="loading">
        <LoadingSpinner message="Ürün yükleniyor..." />
      </div>
      <div v-else-if="error">
        <ErrorMessage :message="error" />
      </div>
      <div v-else-if="successMessage">
        <SuccessMessage :message="successMessage" />
      </div>
      <form v-else @submit.prevent="urunGuncelle" class="space-y-6">
        <div>
          <label for="seriNo" class="block text-sm font-medium text-gray-700">
            Seri No
          </label>
          <div class="mt-1">
            <input
              type="text"
              id="seriNo"
              v-model="urun.seriNo"
              required
              @blur="validateSeriNo"
              :class="{'border-red-300 focus:ring-red-500 focus:border-red-500': errors.seriNo}"
              class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
            />
            <FormError :error="errors.seriNo" />
          </div>
        </div>

        <div>
          <label for="uretimTarihi" class="block text-sm font-medium text-gray-700">
            Üretim Tarihi
          </label>
          <div class="mt-1">
            <input
              type="date"
              id="uretimTarihi"
              v-model="urun.uretimTarihi"
              required
              @blur="validateUretimTarihi"
              :class="{'border-red-300 focus:ring-red-500 focus:border-red-500': errors.uretimTarihi}"
              class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
            />
            <FormError :error="errors.uretimTarihi" />
          </div>
        </div>

        <div>
          <label for="kalitePuani" class="block text-sm font-medium text-gray-700">
            Kalite Puanı
          </label>
          <div class="mt-1">
            <input
              type="number"
              id="kalitePuani"
              v-model="urun.kalitePuani"
              min="0"
              max="100"
              required
              @blur="validateKalitePuani"
              :class="{'border-red-300 focus:ring-red-500 focus:border-red-500': errors.kalitePuani}"
              class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
            />
            <FormError :error="errors.kalitePuani" />
          </div>
        </div>

        <div>
          <label for="durum" class="block text-sm font-medium text-gray-700">
            Durum
          </label>
          <div class="mt-1">
            <select
              id="durum"
              v-model="urun.durum"
              required
              @blur="validateDurum"
              :class="{'border-red-300 focus:ring-red-500 focus:border-red-500': errors.durum}"
              class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
            >
              <option value="Aktif">Aktif</option>
              <option value="Pasif">Pasif</option>
            </select>
            <FormError :error="errors.durum" />
          </div>
        </div>

        <div class="flex justify-end space-x-3">
          <router-link
            to="/"
            class="inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
          >
            İptal
          </router-link>
          <button
            type="submit"
            :disabled="loading || hasErrors"
            class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
          >
            Güncelle
          </button>
        </div>
      </form>
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
  name: 'UrunDuzenle',
  components: {
    LoadingSpinner,
    ErrorMessage,
    SuccessMessage,
    FormError
  },
  data() {
    return {
      urun: {
        id: '',
        seriNo: '',
        uretimTarihi: '',
        kalitePuani: '',
        durum: ''
      },
      errors: {
        seriNo: '',
        uretimTarihi: '',
        kalitePuani: '',
        durum: ''
      },
      loading: true,
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
    validateForm() {
      this.validateSeriNo()
      this.validateUretimTarihi()
      this.validateKalitePuani()
      this.validateDurum()
      return !this.hasErrors
    },
    async urunGetir() {
      try {
        const response = await axios.get(`http://localhost:5176/api/Urun/${this.$route.params.id}`)
        this.urun = {
          ...response.data,
          uretimTarihi: response.data.uretimTarihi.split('T')[0]
        }
      } catch (err) {
        this.error = 'Ürün yüklenirken bir hata oluştu: ' + err.message
      } finally {
        this.loading = false
      }
    },
    async urunGuncelle() {
      if (!this.validateForm()) {
        return
      }

      this.loading = true
      this.error = null
      this.successMessage = null

      try {
        await axios.put(`http://localhost:5176/api/Urun/${this.urun.id}`, this.urun)
        this.successMessage = 'Ürün başarıyla güncellendi'
        setTimeout(() => {
          this.$router.push('/')
        }, 2000)
      } catch (err) {
        this.error = 'Ürün güncellenirken bir hata oluştu: ' + err.message
      } finally {
        this.loading = false
      }
    }
  },
  created() {
    this.urunGetir()
  }
}
</script>