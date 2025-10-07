<template>
  <div v-if="error" class="mt-1 text-sm text-red-600">
    {{ error }}
  </div>
</template>

<script>
export default {
  name: 'FormError',
  props: {
    error: {
      type: String,
      default: ''
    }
  }
}
</script>