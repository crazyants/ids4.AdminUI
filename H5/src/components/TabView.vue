<template>
  <el-container>
    <el-header height="auto">
      <div>{{CurTabIndex}}</div> 
      <span v-for="(tab,index) in Tabs" class="el-tag" :class="{'active':CurTabIndex==index}" @click="ActiveTab(index)" :key="tab.path">{{tab.title}}<span v-if="tab.closable" @click="DelTab(index,$event)" class="el-icon-close"></span></span>
    </el-header>
    <el-main>
      <keep-alive>
        <router-view/>
      </keep-alive>
    </el-main>
  </el-container>
</template>

<script>
import { mapState,mapMutations } from 'vuex'
export default {
  name: "tab-view",
   data() {
      return {
        CurTabName: '2',
        tabIndex: 2
      }
    },
    computed:{
      ...mapState('tab',['CurTabIndex','Tabs'])
    },
    methods: {
      ...mapMutations('tab',['ActiveTab']),
      DelTab(index,e) {
        e.stopPropagation();
        this.$store.commit('tab/DelTab',index)
      }
    }
};
</script>

<style>

</style>