
export default {
    namespaced: true,
    state: {
        CurTabIndex: 1,
        Tabs: [{
            title: '首页',
            path: '/home/index',
            closable: false
        }]
    },
    mutations: {
        OpenTab(state, tab) {
            tab.closable = true;
            const exists = state.Tabs.some(t => t.path == tab.path);
            if (!exists) state.Tabs.push(tab);
        },
        DelTab(state, index) {
            state.Tabs.splice(index, 1);
            if (state.CurTabIndex === index) {
                state.CurTabIndex--;
                //页面跳转
                console.log(this)
            }
            else {
                if (index < state.CurTabIndex) state.CurTabIndex--;
            }
        }
    },
    actions: {

    }
}
