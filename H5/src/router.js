import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'root',
      redirect:'home/index'
    },
    {
      path: '/home',
      name: 'home',
      redirect:'home/index',
      // component: () => import(/* webpackChunkName: "about" */ './views/Home.vue'),
      component:  resolve => require(['./views/Home.vue'],resolve),
      children:[
        {
          path: '/home/index',
          name: 'home.index',
          meta :{title:"首页"},
          // component: () => import(/* webpackChunkName: "about" */ './views/Tabs/Index.vue')
          component:  resolve => require(['./views/Tabs/Index.vue'],resolve),
        },
        {
          path: '/home/test',
          name: 'home.test',
          meta :{title:"测试tab"},//此处为规则配置页面 title为Tab显示的标题
          // component: () => import(/* webpackChunkName: "about" */ './views/Tabs/Test.vue')
          component:  resolve => require(['./views/Tabs/Test.vue'],resolve),
        },
        {
          path: '/home/hasparam/:name',
          name: 'home.hasparam',
          meta :{title:"带参数"},//此处为规则配置页面 title为Tab显示的标题
          // component: () => import(/* webpackChunkName: "about" */ './views/Tabs/HasParam.vue'),
          component: resolve => require(['./views/Tabs/HasParam.vue'],resolve),
          props:true
        },
        {
          path: '/home/role',
          name: 'home.role',
          meta :{title:"角色管理"},//此处为规则配置页面 title为Tab显示的标题
          // component: () => import(/* webpackChunkName: "about" */ './views/Tabs/HasParam.vue'),
          component: resolve => require(['./views/Tabs/RoleManagement.vue'],resolve),
          props:true
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
