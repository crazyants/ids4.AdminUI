import Vue from 'vue'

Vue.mixin({
    activated() {
      this.$store.commit('tab/OpenTab',{path:this.$route.fullPath,title:this.$route.meta.title,component:this})
    },
    beforeRouteUpdate(to,from,next){
      console.log("update");
      next()
    }
  })