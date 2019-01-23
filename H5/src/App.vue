<template>
  <keep-alive exclude='login'>
    <router-view/>
  </keep-alive>
</template>

<script>
export default {
  data() {
    return {
      transition: 'test'
    }
  },
}
</script>


<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
.infinite-scroll-preloader .preloader {
  width: 1rem;
  height: 1rem;
}

.loadtext {
  display: inline-block;
  padding: 0 0.3rem 0.3rem 0;
  font-size: 0.6rem;
  vertical-align: middle;
}

.text-blue {
  color: #0894ec;
}
.text-red {
  color: #f6383a;
}
.text-green {
  color: #4cd964;
}
.text-orange {
  color: #ff6600;
}
</style>
