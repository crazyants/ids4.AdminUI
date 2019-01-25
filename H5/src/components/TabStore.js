
import router from '../router'

const removecomponent = (component) => {
    if (component.$vnode && component.$vnode.data.keepAlive) {
        if (component.$vnode.parent && component.$vnode.parent.componentInstance && component.$vnode.parent.componentInstance.cache) {
            if (component.$vnode.componentOptions) {
                var key = component.$vnode.key == null
                    ? component.$vnode.componentOptions.Ctor.cid + (component.$vnode.componentOptions.tag ? `::${component.$vnode.componentOptions.tag}` : '')
                    : component.$vnode.key;
                var cache = component.$vnode.parent.componentInstance.cache;
                var keys = component.$vnode.parent.componentInstance.keys;
                if (cache[key]) {
                    if (keys.length) {
                        var index = keys.indexOf(key);
                        if (index > -1) {
                            keys.splice(index, 1);
                        }
                    }
                    delete cache[key];
                    component.$destroy();
                }
            }
        }
    }
   
}

export default {
    namespaced: true,
    state: {
        isRouterAlive:true,
        CurTabIndex: 1,
        Tabs: [{
            title: '首页',
            path: '/home/index',
            closable: false
        }]
    },
    mutations: {
        ActiveTab(state, index) {
            router.push(state.Tabs[index].path);
        },
        OpenTab(state, tab) {
            tab.closable = true;
            const item = state.Tabs.find(t => t.path == tab.path);
            if (item) {
                state.CurTabIndex = state.Tabs.indexOf(item);
            }
            else {
                state.Tabs.push(tab);
                state.CurTabIndex = state.Tabs.length - 1;
            }
        },
        DelTab(state, index) {
            removecomponent(state.Tabs[index].component);//删除缓存和销毁组件
            state.Tabs.splice(index, 1);
            if (state.CurTabIndex === index) {
                state.CurTabIndex--;
                //页面跳转
                router.push(state.Tabs[state.CurTabIndex].path);
            }
            else {
                if (index < state.CurTabIndex) state.CurTabIndex--;
            }
        },
        DelCache(state,path){
            const item = state.Tabs.find(t => t.path == path);
            removecomponent(item.component);
        },
        Flush(state){//刷新当前路由
            if(state.isRouterAlive){
                removecomponent(state.Tabs[state.CurTabIndex].component);//删除缓存和销毁组件
            }
            state.isRouterAlive=!state.isRouterAlive;
        }
    },
    actions: {
        reflush ({commit,state}) {//刷新当前路由
            commit('Flush');
            return new Promise((resolve, reject) => {
                setTimeout(() => {
                  commit('Flush');
                  resolve()
                }, 0)
              })
        }
    }
}
