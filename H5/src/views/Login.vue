<template>
  <div>
   Login
  </div>
</template>

<script>
export default {
  name: "login",
  data() {
    return {
      option: {
        remeber:true
      }
    }
  },
  methods: {
      login() {
          this.$router.push('/home');
      }
  },
};
</script>

<style scoped>
</style>