import { createRouter, createWebHistory } from 'vue-router'
import UrunListesi from '../views/UrunListesi.vue'
import UrunEkle from '../views/UrunEkle.vue'
import UrunDuzenle from '../views/UrunDuzenle.vue'
import UrunTipiListesi from '../views/UrunTipiListesi.vue'
import UrunTipiEkle from '../views/UrunTipiEkle.vue'
import UrunTipiDuzenle from '../views/UrunTipiDuzenle.vue'
import IsEmriListesi from '../views/IsEmriListesi.vue'
import IsEmriEkle from '../views/IsEmriEkle.vue'
import IsEmriDuzenle from '../views/IsEmriDuzenle.vue'
import ExcelIslemleri from '../views/ExcelIslemleri.vue'
import HavaDurumu from '../views/HavaDurumu.vue'

const routes = [
  {
    path: '/',
    name: 'UrunListesi',
    component: UrunListesi
  },
  {
    path: '/urun-ekle',
    name: 'UrunEkle',
    component: UrunEkle
  },
  {
    path: '/urun-duzenle/:id',
    name: 'UrunDuzenle',
    component: UrunDuzenle
  },
  {
    path: '/urun-tipleri',
    name: 'UrunTipiListesi',
    component: UrunTipiListesi
  },
  {
    path: '/urun-tipi-ekle',
    name: 'UrunTipiEkle',
    component: UrunTipiEkle
  },
  {
    path: '/urun-tipi-duzenle/:id',
    name: 'UrunTipiDuzenle',
    component: UrunTipiDuzenle
  },
  {
    path: '/is-emirleri',
    name: 'IsEmriListesi',
    component: IsEmriListesi
  },
  {
    path: '/is-emri-ekle',
    name: 'IsEmriEkle',
    component: IsEmriEkle
  },
  {
    path: '/is-emri-duzenle/:id',
    name: 'IsEmriDuzenle',
    component: IsEmriDuzenle
  },
  {
    path: '/excel-islemleri',
    name: 'ExcelIslemleri',
    component: ExcelIslemleri
  },
  {
    path: '/hava-durumu',
    name: 'HavaDurumu',
    component: HavaDurumu
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router