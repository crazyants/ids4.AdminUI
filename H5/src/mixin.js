import Vue from 'vue'

Vue.mixin({
    activated() {
    //   if (this.$$top) {
    //     $(this.$refs.content).scrollTop(this.$$top);
    //   }
        console.log("actived")
        this.$store.commit('tab/OpenTab',{path:this.$route.fullPath,title:this.$route.meta.title})
        console.log(this.$route)
    },
    beforeRouteLeave(to, from, next) {
      // if (this.$refs.content) {
      //   this.$$top = $(this.$refs.content).scrollTop()
      // }
      // else if (from.name == 'home') {
      //   Object.keys(this.$refs).forEach(key => {
      //     const comp = this.$refs[key];
      //     comp.$$top = $(comp.$refs.content).scrollTop()
      //   });
      // }
  
      // if (from.name=='home' && to.name=='login') {//此处判断是如果返回上一层，你可以根据自己的业务更改此处的判断逻辑，酌情决定是否摧毁本层缓存。
      //   if (this.$vnode && this.$vnode.data.keepAlive) {
      //     if (this.$vnode.parent && this.$vnode.parent.componentInstance && this.$vnode.parent.componentInstance.cache) {
      //       if (this.$vnode.componentOptions) {
      //         var key = this.$vnode.key == null
      //           ? this.$vnode.componentOptions.Ctor.cid + (this.$vnode.componentOptions.tag ? `::${this.$vnode.componentOptions.tag}` : '')
      //           : this.$vnode.key;
      //         var cache = this.$vnode.parent.componentInstance.cache;
      //         var keys = this.$vnode.parent.componentInstance.keys;
      //         if (cache[key]) {
      //           if (keys.length) {
      //             var index = keys.indexOf(key);
      //             if (index > -1) {
      //               keys.splice(index, 1);
      //             }
      //           }
      //           delete cache[key];
      //         }
      //       }
      //     }
      //   }
      //   this.$destroy();
      // }
      next();
    }
  })