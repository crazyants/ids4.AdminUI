
import 'babel-polyfill'
import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import './assets/css/reset.css'
import './assets/css/index.css'
import http from './plugin/http.js'
Vue.config.productionTip = false

Vue.use(ElementUI);

Vue.use(http);

import './mixin'
import Oidc from 'oidc-client'
import mgr from './plugin/oidc'

var main = async ()=>{
  var user = await mgr.getUser();
  if(!user){
    await (new Oidc.UserManager().signinRedirectCallback().catch(e=>{}));//回调认证
    user = await mgr.getUser(); //重试
  } 
  if (user){
    new Vue({
      router,
      store,
      render: h => h(App)
    }).$mount('#app')
  }
  else mgr.signinRedirect();
}

main();


