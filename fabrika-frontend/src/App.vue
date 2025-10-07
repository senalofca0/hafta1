<template>
  <div class="min-h-screen bg-gray-50">
    <nav class="bg-white shadow-lg">
      <div class="max-w-7xl mx-auto px-4">
        <div class="flex justify-between h-16">
          <div class="flex">
            <div class="flex-shrink-0 flex items-center">
              <router-link to="/" class="flex items-center space-x-2">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-indigo-600" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M4 4a2 2 0 012-2h8a2 2 0 012 2v12a1 1 0 01-1 1H5a1 1 0 01-1-1V4zm3 1h6v4H7V5zm6 6H7v2h6v-2z" clip-rule="evenodd" />
                  <path d="M2 6a2 2 0 012-2h12a2 2 0 012 2v8a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" />
                </svg>
                <span class="text-xl font-bold text-indigo-900">Fabrika Yönetimi</span>
              </router-link>
            </div>
            <div class="hidden sm:ml-8 sm:flex sm:space-x-8">
              <router-link
                to="/"
                class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors duration-150"
                :class="[$route.path === '/' ? 'border-indigo-500 text-indigo-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1.5" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M7 3a1 1 0 000 2h6a1 1 0 100-2H7zM4 7a1 1 0 011-1h10a1 1 0 110 2H5a1 1 0 01-1-1zM2 11a2 2 0 012-2h12a2 2 0 012 2v4a2 2 0 01-2 2H4a2 2 0 01-2-2v-4z" />
                </svg>
                Ürünler
              </router-link>
              <router-link
                to="/urun-tipleri"
                class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors duration-150"
                :class="[$route.path === '/urun-tipleri' ? 'border-indigo-500 text-indigo-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1.5" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z" />
                  <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm3 4a1 1 0 000 2h.01a1 1 0 100-2H7zm3 0a1 1 0 000 2h3a1 1 0 100-2h-3zm-3 4a1 1 0 100 2h.01a1 1 0 100-2H7zm3 0a1 1 0 100 2h3a1 1 0 100-2h-3z" clip-rule="evenodd" />
                </svg>
                Ürün Tipleri
              </router-link>
              <router-link
                to="/is-emirleri"
                class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors duration-150"
                :class="[$route.path === '/is-emirleri' ? 'border-indigo-500 text-indigo-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1.5" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd" />
                </svg>
                İş Emirleri
              </router-link>
              <router-link
                to="/excel-islemleri"
                class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors duration-150"
                :class="[$route.path === '/excel-islemleri' ? 'border-indigo-500 text-indigo-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1.5" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M6 6V4c0-1.1.9-2 2-2h8a2 2 0 012 2v12a2 2 0 01-2 2H8a2 2 0 01-2-2v-2h2v2h8V4H8v2H6zm7.5 3v-3h-2v3h-3v2h3v3h2v-3h3V9h-3z" clip-rule="evenodd" />
                </svg>
                Excel İşlemleri
              </router-link>
              <router-link
                to="/hava-durumu"
                class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors duration-150"
                :class="[$route.path === '/hava-durumu' ? 'border-indigo-500 text-indigo-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1.5" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M5.5 16a3.5 3.5 0 01-.369-6.98 4 4 0 117.753-1.977A4.5 4.5 0 1113.5 16h-8z" />
                </svg>
                Hava Durumu
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <router-view v-slot="{ Component }">
        <transition
          name="page"
          mode="out-in"
          @before-enter="beforeEnter"
          @enter="enter"
          @leave="leave"
        >
          <component :is="Component" />
        </transition>
      </router-view>
    </main>
  </div>
</template>

<script>
import gsap from 'gsap'

export default {
  name: 'App',
  methods: {
    beforeEnter(el) {
      el.style.opacity = 0
      el.style.transform = 'translateY(20px)'
    },
    enter(el, done) {
      gsap.to(el, {
        duration: 0.3,
        opacity: 1,
        y: 0,
        onComplete: done
      })
    },
    leave(el, done) {
      gsap.to(el, {
        duration: 0.2,
        opacity: 0,
        y: -20,
        onComplete: done
      })
    }
  }
}
</script>

<style>
.page-enter-active,
.page-leave-active {
  transition: opacity 0.3s, transform 0.3s;
}

.page-enter-from,
.page-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
</style>
