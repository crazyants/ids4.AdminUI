
export default {
  state: {
    CurTabIndex: 0,
    Tabs: [{
      title: '首页',
      path:'index',
      closable:false
    }, {
      title: 'Tab 2',
      path:'other',
      closable:true
    }]
  },
  mutations: {
    AddTab(state,tab){
      state.Tabs.push(tab);
    },
    DelTab(state,index){
        state.Tabs.splice(idnex,1);
        state.CurTabIndex = 0;
    }
  },
  actions: {

  }
}
