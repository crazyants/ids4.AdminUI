import Vue from 'vue'
//PC
Vue.mixin({
  mounted(){
    if(this.wrapClass=="scrollbar-wrapper-y"){
      this.$$scrollbar_resize = ()=>{
        this.update();
      }
      window.addEventListener("resize",this.$$scrollbar_resize);
    }
  },
  beforeDestroy(){
    if(this.wrapClass=="scrollbar-wrapper-y"){
      window.removeEventListener("resize",this.$$scrollbar_resize);
    }
  }
})

const whiteList = ['login']// 免登录白名单
Vue.mixin({
  activated() {
    //滚动条位置（ 注意：el-scrollbar 一定要是根节点 ）
    if (this.$$top&&this.$el.className=="el-scrollbar") {
      this.$el.firstElementChild.scrollTop = this.$$top;
    }
  },
  beforeRouteLeave(to, from, next) {
    //滚动条位置（ 注意：el-scrollbar 一定要是根节点 ）
    if(this.$el.className=="el-scrollbar"){
      this.$$top = this.$el.firstElementChild.scrollTop;
    }

    next();
  },
    beforeRouteEnter(to, from, next){
      if (whiteList.indexOf(to.name) !== -1) { // 在免登录白名单，直接进入
        next()
      }
      else{
        // if(localStorage.getItem("_token")){
            next()
        // }
        // else
        //   next('/login')
      }
    }
  })