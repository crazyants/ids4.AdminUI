<template>
  <div>
    <span
      v-for="(tab,index) in Tabs"
      class="el-tag"
      :class="{'active':CurTabIndex==index}"
      @click="ActiveTab(index)"
      :key="tab.path"
    >
      {{tab.title}}
      <span v-if="tab.closable" @click="DelTab(index,$event)" class="el-icon-close"></span>
    </span>
    <keep-alive>
      <router-view v-if="isRouterAlive" ref='cur'/>
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

<style>
</style>