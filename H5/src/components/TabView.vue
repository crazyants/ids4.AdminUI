<template>
  <el-container>
    <el-header height="auto">
      <div>{{CurTabIndex}}</div> 
      <span v-for="(tab,index) in Tabs" class="el-tag" :key="tab.path">{{tab.title}}<span v-if="tab.closable" @click="DelTab(index)" class="el-icon-close"></span></span>
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
      ...mapMutations('tab',['DelTab']),
      addTab(targetName) {
        let newTabName = ++this.tabIndex + '';
        this.Tabs.push({
          title: 'New Tab',
          name: newTabName,
          content: 'New Tab content'
        });
        this.CurTabName = newTabName;
      },
      removeTab(targetName) {
        let tabs = this.Tabs;
        let activeName = this.CurTabName;
        if (activeName === targetName) {
          tabs.forEach((tab, index) => {
            if (tab.name === targetName) {
              let nextTab = tabs[index + 1] || tabs[index - 1];
              if (nextTab) {
                activeName = nextTab.name;
              }
            }
          });
        }
        
        this.CurTabName = activeName;
        this.Tabs = tabs.filter(tab => tab.name !== targetName);
      }
    }
};
</script>

<style>
.hometab.el-tabs--card>.el-tabs__header .el-tabs__item .el-icon-close{
  position:absolute;
  top: 0;
  right: 0;
}
.hometab.el-tabs--card>.el-tabs__header .el-tabs__item.is-active.is-closable .el-icon-close{
width:0px;
}
.hometab.el-tabs--card>.el-tabs__header .el-tabs__item.is-closable:hover .el-icon-close
{
    width: 14px
}
.el-tabs--card>.el-tabs__header{
    /* height: 60px; */
    margin: 0;
}

  .el-tabs__content{
    display: none;
  }
</style>