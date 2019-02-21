<template>
  <el-container id="content_box">
    <el-header>
      <HeaderBar :toggle-side-bar="toggleSideBar" :is-active="isCollapse"></HeaderBar>
    </el-header>
    <el-container class="aside_box">
      <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <el-aside width="auto">
          <el-menu
            :default-active="activemenu"
            class="el-menu-vertical-demo"
            @select="openTab"
            :collapse="isCollapse"
            :collapse-transition="false"
            background-color="#2e4050"
            text-color="#fff"
            :unique-opened="true"
          >
              <template v-for='(menu1,index1) in menus' >
                  <el-submenu :key="index1" v-if="menu1.children" :index="menu1.index">
                      <template slot="title">
                          <i :class="menu1.icon"></i>
                          <span slot="title">{{menu1.title}}</span>
                      </template>
                      <template v-for='(menu2,index2) in menu1.children' >
                          <el-submenu :key="index2" v-if="menu2.children" :index="menu2.index">
                              <template slot="title">
                                  <i :class="menu2.icon" v-if="menu2.icon"></i>
                                  <span slot="title">{{menu2.title}}</span>
                              </template>
                              <el-menu-item v-for="(menu3,index3) in menu2.children" :key="index3" :index="menu3.index">{{menu3.title}}</el-menu-item>
                          </el-submenu>
                          <el-menu-item :key="index2" v-else :index="menu2.index">
                              <i :class="menu2.icon" v-if="menu2.icon"></i>
                              <span slot="title">{{menu2.title}}</span>
                          </el-menu-item>
                      </template>
                  </el-submenu>
                  <el-menu-item :key="index1" v-else :index="menu1.index">
                      <i :class="menu1.icon"></i>
                      <span slot="title">{{menu1.title}}</span>
                  </el-menu-item>
              </template>
            <!--<el-submenu index="1">-->
              <!--<template slot="title">-->
                <!--<i class="el-icon-menu"></i>-->
                <!--<span slot="title">权限管理</span>-->
              <!--</template>-->
              <!--<el-menu-item index="1-1">系统配置</el-menu-item>-->
              <!--<el-menu-item index="1-2">模块配置</el-menu-item>-->
              <!--<el-menu-item index="1-3">权限配置</el-menu-item>-->
            <!--</el-submenu>-->
            <!--<el-menu-item index="2">-->
              <!--<i class="el-icon-document"></i>-->
              <!--<span slot="title">角色管理</span>-->
            <!--</el-menu-item>-->
            <!--<el-menu-item index="3">-->
              <!--<i class="el-icon-location"></i>-->
              <!--<span slot="title">人员管理</span>-->
            <!--</el-menu-item>-->
          </el-menu>
        </el-aside>
      </el-scrollbar>
      <el-main >
        <tab-view/>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
// @ is an alias to /src
import { mapActions } from "vuex";
import TabView from "../components/TabView.vue";
import menus from './menuconfig'
import HeaderBar from "../components/HeaderBar/index";
export default {
  name: "home",
  data: function() {
    return {
      isCollapse: false,
      activemenu:"",
      mainboxH: 0,
        menus:[]
    };
  },
  components: {
    TabView,
    HeaderBar
  },
  mounted() {
    this.menus.push(...menus);
    const fullpath = this.$route.fullPath;
    this.menus.forEach((menu1,index1)=>{
      if(menu1.children){
        menu1.children.forEach((menu2,index2)=>{
            if(menu2.children){
                menu2.children.forEach((menu3,index3)=>{
                    if(menu3.fullPath==fullpath) this.activemenu = menu3.index;
                })
            }
            else if(menu2.fullPath==fullpath) this.activemenu = menu2.index;
        })
      }
      else if(menu1.fullPath==fullpath) this.activemenu = menu1.index;
    })
  },
  methods: {
      ...mapActions("tab", ["reflush"]),
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    toggleSideBar() {
      this.isCollapse = !this.isCollapse;
    },
    openTab(key, keyPath) {
        var items= this.menus;
        var item = null;
        var ids = key.split('.').map(i=>parseInt(i));
        ids.forEach(i=>{
            if(items[i].children) items = items[i].children;
            else item = items[i];
        });
        this.$router.push(item.fullPath);
        // if(item.fullPath){
        //     if(this.$router.currentRoute.fullPath==item.fullPath){
        //         this.reflush();
        //     }
        //     else {
        //         this.$store.commit("tab/DelCache", item.fullPath);
        //         this.$router.push(item.fullPath);
        //     }
        // }
        // else console.warn('该菜单未配置路由')
    }
  }
};
</script>

<style type='scss' scoped>
#content_box {
  height: 100%;
}
.el-header {
  background-color: #409eff;
  color: #333;
  line-height: 50px;
  height: 50px !important;
  padding: 0;
}
.aside_box {
  height: 100%;
  background-color: #2e4050;
  color: #333;
}
.aside_box>.el-scrollbar{
  flex-grow: 0;/* 不索取父容器空间 默认0 */
  flex-shrink: 0;/* 父容器空间不够，不压缩 默认1 */
}
.el-menu-vertical-demo:not(.el-menu--collapse) {
  min-width: 180px;/* 左边导航宽度 */
  min-height: 100%;
}
.el-main {
  background-color: #fff;
  color: #333;
  height: 100%;
  padding: 0;
  position: relative;
}
</style>