import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'root',
      redirect:'home/test'
    },
    {
      path: '/home',
      name: 'home',

      // component: Home
      component: () => import(/* webpackChunkName: "about" */ './views/Home.vue'),
      children:[
        {
          path: '/home/index',
          name: 'home.index',
          meta :{title:"首页"},
          component: () => import(/* webpackChunkName: "about" */ './views/Tabs/Index.vue')
        },
        {
          path: '/home/test',
          name: 'home.test',
          meta :{title:"测试"},//此处为规则配置页面 title为Tab显示的标题
          component: () => import(/* webpackChunkName: "about" */ './views/Tabs/Index2.vue')
        }
      ]
    },
    {
      path: '/login',
      name: 'login',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "about" */ './views/Login.vue')
    }
  ]
})
