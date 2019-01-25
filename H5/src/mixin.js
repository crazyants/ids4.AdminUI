import Vue from 'vue'

const whiteList = ['login']// 免登录白名单
Vue.mixin({
    activated() {
      var title="新页面";
      if(this.$route.meta&&this.$route.meta.title) title = this.$route.meta.title;
      this.$store.commit('tab/OpenTab',{routername:this.$route.name,routerparams:this.$route.params,title:title,component:this})
    },
    beforeRouteEnter(to, from, next){
      if (whiteList.indexOf(to.name) !== -1) { // 在免登录白名单，直接进入
        next()
      }
      else{
        if(localStorage.getItem("_token"))
          next()
        else
          next('/login')
      }
    }
  })