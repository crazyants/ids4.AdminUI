<template>
  <el-container>
    <el-header height="auto">
      <el-tabs class="hometab" v-model="CurTabName" type="card" @tab-remove="removeTab">
        <el-tab-pane
          v-for="item in Tabs"
          :key="item.name"
          :label="item.title"
          :name="item.name"
          :closable="item.closable"
        ></el-tab-pane>
      </el-tabs>
    </el-header>
    <el-main>
      <keep-alive exclude="login">
        <router-view/>
      </keep-alive>
    </el-main>
  </el-container>
</template>

<script>
export default {
  name: "tab-view",
   data() {
      return {
        CurTabName: '2',
        Tabs: [{
          title: 'Index',
          name: '1',
          closable:false
        }, {
          title: 'Tab 2',
          name: '2',
          closable:true
        }],
        tabIndex: 2
      }
    },
    methods: {
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