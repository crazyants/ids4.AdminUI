
import router from '../router' //app路由配置文件
import Vue from 'vue'

var maxtab = 999;//tab打开最大数量配置
var defaulttab = {//默认打开页面
    title: '首页',
    fullPath: '/home/index',//完整路径
    closable: false
};

Vue.mixin({
  created() {
    if(!this.$vnode||!this.$vnode.parent||!this.$vnode.parent.componentInstance.cache) return;
    var title="新页面";
    if(this.$route.meta&&this.$route.meta.title) title = this.$route.meta.title;
    else{
        console.warn("您还未配置该路由的Tab页面显示的标题！{meta:{tile:'页面名称'}}")
    }
    if(!this.$route.name) throw "您未配置该页面路由名称 {name}"
    this.$store.commit('tab/OpenTab',{fullPath:this.$route.fullPath,title:title,component:this})
  }
})

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
                }
            }
        }
    }
    component.$destroy();
}



export default {
    namespaced: true,
    state: {
        isRouterAlive:true,
        CurTabIndex: 1,
        Tabs: [defaulttab]
    },
    mutations: {
        ActiveTab(state, index) {
            // var routerconfig = { name: state.Tabs[index].routername };
            // if(state.Tabs[index].routerparams) routerconfig.params = state.Tabs[index].routerparams;
            router.push(state.Tabs[index].fullPath);
            state.CurTabIndex = index;
        },
        OpenTab(state, tab) {//路由拦截触发,获取meta里的title
            tab.closable = true;
            const item = state.Tabs.find(t => t.fullPath == tab.fullPath);
            if (item) {
                if(tab.component) item.component = tab.component;
                state.CurTabIndex = state.Tabs.indexOf(item);
            }
            else {
                state.Tabs.push(tab);
                if(state.Tabs.length>maxtab){
                    removecomponent(state.Tabs[1].component);
                    state.Tabs.splice(1,1);
                }
                state.CurTabIndex = state.Tabs.length - 1;
            }
        },
        ModifyTile(state, tab){//自定标题
            const item = state.Tabs.find(t => t.fullPath == tab.fullPath);
            item.title = tab.title;
        },
        DelTab(state, index) {
            removecomponent(state.Tabs[index].component);//删除缓存和销毁组件
            state.Tabs.splice(index, 1);
            if (state.CurTabIndex === index) {
                if(state.CurTabIndex===0){
                    if(!state.Tabs.length){ router.push('/');return;}
                }
                else{
                    if(state.Tabs.length===1||state.CurTabIndex!==1)
                        state.CurTabIndex--;
                }
                router.push(state.Tabs[state.CurTabIndex].fullPath);//页面跳转
                
            }
            else {
                if (index < state.CurTabIndex) state.CurTabIndex--;
            }
        },
        DelCache(state,fullPath){
            const item = state.Tabs.find(t => t.fullPath == fullPath);
            if(item&&item.component) removecomponent(item.component);
        },
        Flush(state){//刷新当前路由
            if(state.isRouterAlive){
                removecomponent(state.Tabs[state.CurTabIndex].component);//删除缓存和销毁组件
            }
            state.isRouterAlive=!state.isRouterAlive;
        },
        exchange(state,{before,after}){//改变顺序
           var activeitem = state.Tabs[state.CurTabIndex];
           const beforeitem =  state.Tabs.splice(before,1)[0];
           state.Tabs.splice(after,0,beforeitem);
           state.CurTabIndex = state.Tabs.indexOf(activeitem);
        },
        closeAndTo(state,{fullPath,flush}){//关闭当前页跳转到指定页面,并执行指定函数
            removecomponent(state.Tabs[state.CurTabIndex].component);//删除缓存和销毁组件
            state.Tabs.splice(state.CurTabIndex, 1);
            const item = state.Tabs.find(t => t.fullPath == fullPath);
            if(item&&item.component&&item.component[flush]) item.component[flush]();
            router.push(fullPath);//页面跳转
        },
        DelOther(state){//除当前激活页其他都关闭
            var activeitem = state.Tabs[state.CurTabIndex];
            let i = state.Tabs.length-1;
            while(i>=0){
                if(state.Tabs[i]!=activeitem&&state.Tabs[i].closable){
                    const item = state.Tabs.splice(i,1)[0];
                    removecomponent(item.component);
                }
                i--;
            }
            state.CurTabIndex = state.Tabs.indexOf(activeitem);
        },
        DelAll(state){//关闭所有页面
            var activeitem = state.Tabs[state.CurTabIndex];
            let i = state.Tabs.length-1;
            while(i>=0){
                if(state.Tabs[i].closable){
                    const item = state.Tabs.splice(i,1)[0];
                    removecomponent(item.component);
                }
                i--;
            }
            state.CurTabIndex = state.Tabs.indexOf(activeitem);
            if(state.CurTabIndex==-1){ 
                state.CurTabIndex=0; 
                router.push(state.Tabs[0].fullPath);
            }
        },
        Fixed(state,index){//固定
           if(index) state.Tabs[index].closable = !state.Tabs[index].closable;
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
