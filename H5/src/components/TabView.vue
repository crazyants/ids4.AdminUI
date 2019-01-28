<template>
  <div>
    <div class='tab_box'>
       <span
               v-for="(tab,index) in Tabs"
               class="el-tag"
               :class="{'active':CurTabIndex==index}"
               @click="ActiveTab(index)"
               :key="tab.path"
       >
       <s class='tab_rect'></s>
      {{tab.title}}
      <i v-if="tab.closable" @click="DelTab(index,$event)" class="el-icon-close"></i>
    </span>
    </div>
    <keep-alive>
      <el-main>
        <router-view v-if="isRouterAlive" ref='cur'/>
      </el-main>
    </keep-alive>
  </div>
</template>

<script>
import { mapState, mapMutations } from "vuex";
export default {
  name: "tab-view",
  data() {
    return {
      CurTabName: "2"
    };
  },
  computed: {
    ...mapState("tab", ["CurTabIndex", "Tabs","isRouterAlive"])
  },
  methods: {
    ...mapMutations("tab", ["ActiveTab"]),
    DelTab(index, e) {
      e.stopPropagation();
      this.$store.commit("tab/DelTab", index);
    },
    reload () {
    }  
  },
  provide(){
    return {
      reload: this.reload
    }
  }
};
</script>

<style lang='scss' scoped>
  .tab_box {
    border-bottom: 2px solid #304156;
    background-color: #e9eef3;
    .el-tag{
      cursor: pointer;
      height: 40px;
      line-height: 40px;
      border-right: solid 1px #ddd;
      color: #475059;
      background: #fff;
      padding: 0 10px;
      font-size: 12px;
      border-radius: 0;
      i{
        color: #495060;
        &:hover {
          background-color: #b4bccc;
          color: #fff;
        }
      }
    }
    .el-tag.active{
      background-color: #304156;
      color: #fff;
      border-color: #304156;
      margin-bottom: -1px;
      i{
        color: #fff;
      }
      .tab_rect {
        content: "";
        background: #fff;
        display: inline-block;
        width: 8px;
        height: 8px;
        border-radius: 50%;
        position: relative;
        margin-right: 2px;
      }
    }
  }

</style>