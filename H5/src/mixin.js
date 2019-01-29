import Vue from 'vue'
const whiteList = ['login']// 免登录白名单
Vue.mixin({
    beforeRouteEnter(to, from, next){
      if (whiteList.indexOf(to.name) !== -1) { // 在免登录白名单，直接进入
        next()
      }
      else{
        if(localStorage.getItem("_token")){
            next()
        }
        else
          next('/login')
      }
    }
  })