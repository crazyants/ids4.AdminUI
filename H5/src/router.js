import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  // mode: 'history',
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
          component:  resolve => require(['./views/Tabs/Index.vue'],resolve),
        },
        {
          path: '/home/test',
          name: 'home.test',
          meta :{title:"测试tab"},//此处为规则配置页面 title为Tab显示的标题
          component:  resolve => require(['./views/Tabs/Test.vue'],resolve),
        },
        {
          path: '/home/hasparam/:name',
          name: 'home.hasparam',
          meta :{title:"带参数"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/HasParam.vue'],resolve),
          props:true
        },
        {
          path: '/home/role',
          name: 'home.role',
          meta :{title:"角色管理"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/RoleManagement.vue'],resolve)
        },
        {
          path: '/home/people',
          name: 'home.people',
          meta :{title:"人员管理"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/PeopleManagement.vue'],resolve)
        },
        {
          path: '/home/app',
          name: 'home.app',
          meta :{title:"系统配置"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/AppManagement.vue'],resolve)
        },
        {
          path: '/home/module',
          name: 'home.module',
          meta :{title:"模块配置"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/ModuleManagement.vue'],resolve)
        },
        {
          path: '/home/rolemap/:roleid',
          name: 'home.rolemap',
          meta :{title:"权限分配"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/RolePermissionMap.vue'],resolve)
        },
        {
          path: '/home/client',
          name: 'home.client',
          meta :{title:"客户端管理"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/Client.vue'],resolve)
        },
        {
          path: '/home/apiresource',
          name: 'home.apiresource',
          meta :{title:"资源管理"},//此处为规则配置页面 title为Tab显示的标题
          component: resolve => require(['./views/Tabs/ApiResource.vue'],resolve)
        },
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
