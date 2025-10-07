<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-4xl mx-auto">
      <h1 class="text-2xl font-bold mb-6">Excel İşlemleri</h1>

      <!-- Import Bölümü -->
      <div class="bg-white shadow-md rounded-lg p-6 mb-8">
        <h2 class="text-xl font-semibold mb-4">Veri İçe Aktarma</h2>
        
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2">
            İçe Aktarılacak Veri Türü
          </label>
          <select
            v-model="importType"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option value="">Seçiniz</option>
            <option value="urunler">Ürünler</option>
            <option value="uruntipleri">Ürün Tipleri</option>
            <option value="isemirleri">İş Emirleri</option>
          </select>
        </div>

        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2">
            Excel Dosyası
          </label>
          <input
            type="file"
            @change="handleFileSelect"
            accept=".xlsx"
            class="block w-full text-sm text-gray-500
              file:mr-4 file:py-2 file:px-4
              file:rounded-full file:border-0
              file:text-sm file:font-semibold
              file:bg-indigo-50 file:text-indigo-700
              hover:file:bg-indigo-100"
          >
        </div>

        <button
          @click="importExcel"
          :disabled="!selectedFile || !importType || importing"
          class="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {{ importing ? 'İçe Aktarılıyor...' : 'İçe Aktar' }}
        </button>
      </div>

      <!-- Export Bölümü -->
      <div class="bg-white shadow-md rounded-lg p-6">
        <h2 class="text-xl font-semibold mb-4">Veri Dışa Aktarma</h2>
        
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2">
            Dışa Aktarılacak Veri Türü
          </label>
          <select
            v-model="exportType"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option value="">Seçiniz</option>
            <option value="urunler">Ürünler</option>
            <option value="uruntipleri">Ürün Tipleri</option>
            <option value="isemirleri">İş Emirleri</option>
          </select>
        </div>

        <div v-if="exportType === 'isemirleri'" class="mb-6">
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-gray-700 text-sm font-bold mb-2">
                Başlangıç Tarihi
              </label>
              <input
                type="date"
                v-model="startDate"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              >
            </div>
            <div>
              <label class="block text-gray-700 text-sm font-bold mb-2">
                Bitiş Tarihi
              </label>
              <input
                type="date"
                v-model="endDate"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              >
            </div>
          </div>
        </div>

        <button
          @click="exportExcel"
          :disabled="!exportType || exporting"
          class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {{ exporting ? 'Dışa Aktarılıyor...' : 'Dışa Aktar' }}
        </button>
      </div>

      <!-- Mesajlar -->
      <div class="mt-6">
        <SuccessMessage v-if="successMessage" :message="successMessage" class="mb-4" />
        <ErrorMessage v-if="error" :message="error" />
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import ErrorMessage from '../components/ErrorMessage.vue'
import SuccessMessage from '../components/SuccessMessage.vue'

export default {
  name: 'ExcelIslemleri',
  components: {
    ErrorMessage,
    SuccessMessage
  },
  setup() {
    const importType = ref('')
    const exportType = ref('')
    const selectedFile = ref(null)
    const startDate = ref('')
    const endDate = ref('')
    const importing = ref(false)
    const exporting = ref(false)
    const error = ref(null)
    const successMessage = ref(null)

    const handleFileSelect = (event) => {
      const file = event.target.files[0]
      if (file && file.name.endsWith('.xlsx')) {
        selectedFile.value = file
        error.value = null
      } else {
        error.value = 'Lütfen geçerli bir Excel dosyası (.xlsx) seçin'
        selectedFile.value = null
        event.target.value = null
      }
    }

    const importExcel = async () => {
      if (!selectedFile.value || !importType.value) return

      importing.value = true
      error.value = null
      successMessage.value = null

      const formData = new FormData()
      formData.append('file', selectedFile.value)
      formData.append('importType', importType.value)

      try {
        const response = await fetch('http://localhost:5176/api/Excel/import', {
          method: 'POST',
          body: formData
        })

        if (!response.ok) {
          const errorData = await response.text()
          throw new Error(errorData)
        }

        const result = await response.text()
        successMessage.value = result
        
        // Form'u sıfırla
        importType.value = ''
        selectedFile.value = null
        const fileInput = document.querySelector('input[type="file"]')
        if (fileInput) fileInput.value = null
      } catch (err) {
        error.value = err.message
      } finally {
        importing.value = false
      }
    }

    const exportExcel = async () => {
      if (!exportType.value) return

      exporting.value = true
      error.value = null
      successMessage.value = null

      try {
        let url = `http://localhost:5176/api/Excel/export?exportType=${exportType.value}`
        
        if (exportType.value === 'isemirleri') {
          if (startDate.value) url += `&startDate=${startDate.value}`
          if (endDate.value) url += `&endDate=${endDate.value}`
        }

        const response = await fetch(url)

        if (!response.ok) {
          const errorData = await response.text()
          throw new Error(errorData)
        }

        // Excel dosyasını indir
        const blob = await response.blob()
        const downloadUrl = window.URL.createObjectURL(blob)
        const link = document.createElement('a')
        link.href = downloadUrl
        link.download = `${exportType.value}_${new Date().toISOString().split('T')[0]}.xlsx`
        document.body.appendChild(link)
        link.click()
        document.body.removeChild(link)
        window.URL.revokeObjectURL(downloadUrl)

        successMessage.value = 'Dosya başarıyla indirildi'
        
        // Form'u sıfırla
        exportType.value = ''
        startDate.value = ''
        endDate.value = ''
      } catch (err) {
        error.value = err.message
      } finally {
        exporting.value = false
      }
    }

    return {
      importType,
      exportType,
      selectedFile,
      startDate,
      endDate,
      importing,
      exporting,
      error,
      successMessage,
      handleFileSelect,
      importExcel,
      exportExcel
    }
  }
}
</script>