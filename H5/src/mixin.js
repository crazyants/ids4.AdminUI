import Vue from 'vue'

Vue.mixin({
    activated() {
      var title="新页面";
      if(this.$route.meta&&this.$route.meta.title) title = this.$route.meta.title;
      this.$store.commit('tab/OpenTab',{path:this.$route.fullPath,title:title,component:this})
    }
  })