<template>
  <div class="tabbody"  @dragover="allowDrop($event)" @drop="drop($event)">
    <el-scrollbar class="tab_box scroll-container" wrap-class="scrollbar-wrapper-y" ref="tabbox">
      <span draggable="true" @dragstart="drag($event,index)" 
        v-for="(tab,index) in Tabs"
        class="el-tag"
        :class="{'active':CurTabIndex==index}"
        @click="ActiveTab(index)"
        :key="tab.routername"
      >
        <s class="tab_rect"></s>
        {{tab.title}}
        <i
          v-if="tab.closable"
          @click="DelTab(index,$event)"
          class="el-icon-close"
        ></i>
      </span>
    </el-scrollbar>
    <el-main>
      <keep-alive>
        <router-view v-if="isRouterAlive" :key="$route.fullPath" ref="cur"/>
      </keep-alive>
    </el-main>
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
    ...mapState("tab", ["CurTabIndex", "Tabs", "isRouterAlive"]),
    tabcount() {
      return this.Tabs.length;
    }
  },
  mounted(){
    this.$refs.tabbox.$el.addEventListener("mousewheel",(e)=>{
      if(e.wheelDelta<0) this.$refs.tabbox.$el.firstElementChild.scrollLeft+=10;
      else if(e.wheelDelta>0) this.$refs.tabbox.$el.firstElementChild.scrollLeft-=10;
    })
  },
  methods: {
    ...mapMutations("tab", ["ActiveTab"]),
    DelTab(index, e) {
      e.stopPropagation();
      this.$store.commit("tab/DelTab", index);
    },
    drag(event,dragindex) {
       const dom = event.currentTarget;
       this.$start_x = event.clientX;
       this.$move_x = event.clientX;
       this.$dragindex = dragindex;
       this.$scrollLeft = this.$refs.tabbox.$el.firstElementChild.scrollLeft;
    },
    drop(event) {
        const dx = event.clientX - this.$start_x;
        event.preventDefault();
        const afterindex = this.getafterindex(dx);
        if(afterindex != this.$dragindex){
            this.$store.commit("tab/exchange", {before:this.$dragindex,after:afterindex});
        }
    },
    getafterindex(dx){//根据拖拽位移获取 移动后的index
        dx +=this.$refs.tabbox.$el.firstElementChild.scrollLeft - this.$scrollLeft;//相对滚动条的移动距离
        const tabbox = this.$refs.tabbox.$el.firstChild.firstChild;
        let afterindex = this.$dragindex;
        if(dx<0){//左移
            var i = this.$dragindex -1;
            let dis = -dx;
            while(i>0){
               dis -= tabbox.children[i].clientWidth;
               if(dis>0) afterindex = i;
               else break;
               i--;
            }
        }
        else if(dx>0&&this.$dragindex>0){//右移
            var i = this.$dragindex+1;
            let dis = dx;
            while(i<tabbox.children.length){
                dis -= tabbox.children[i].clientWidth;
                if(dis>0) afterindex = i;
                else break;
                i++;
            }
        }
        return afterindex;
    },
    allowDrop(event) {
        event.preventDefault();
        const dx = event.clientX - this.$move_x;
        this.$refs.tabbox.$el.firstElementChild.scrollLeft+=dx;
        this.$move_x = event.clientX;
    },
    scrollDis(dis){
      if(this.$interval){ clearInterval(this.$interval);this.$interval = null;}
      const el = this.$refs.tabbox.$el.firstElementChild;
      let dv = dis;
      this.$interval = setInterval(()=>{
        if (Math.abs(dv)>1) {
          let d = parseInt(dv/3);
          el.scrollLeft +=d;
          dv = dv - d;
        }
        else{
          el.scrollLeft += dv;
          clearInterval(this.$interval);
          this.$interval = null;
        }
      },25);
    },
    reload() {}
  },
  watch: {
    tabcount() {
      this.$nextTick(() => {
        this.$refs.tabbox.update();
        // if(this.CurTabIndex==this.Tabs.length-1){
        //   const el = this.$refs.tabbox.$el.firstElementChild;
        //   el.scrollLeft = el.scrollWidth - el.clientWidth;
        //   console.log(this.$refs.tabbox.$el.firstElementChild.scrollWidth) 
        //   console.log(this.$refs.tabbox.$el.firstElementChild.clientWidth) 
        // }
      });
    },
    CurTabIndex(){
      this.$nextTick(() => {
        const el = this.$refs.tabbox.$el.firstElementChild;
        const tabbox = this.$refs.tabbox.$el.firstChild.firstChild;
        const actel = tabbox.children[this.CurTabIndex];
        const scrollleft = actel.offsetLeft - parseInt(el.clientWidth/2);
        let dis = 0;
        if(scrollleft<=0){
          dis = 0 - el.scrollLeft;
        }
        else if((el.scrollWidth - actel.offsetLeft)<parseInt(el.clientWidth/2)){
          dis = el.scrollWidth - el.clientWidth - el.scrollLeft;
        }
        else{
          dis = scrollleft - el.scrollLeft;
        }
        console.log(dis);
        if(dis!=0) this.scrollDis(dis);
      });
    }
    // tabcount() {
    //   this.$nextTick(() => {
    //     const tabbox = this.$refs.tabbox;
    //     const totalwidth = tabbox.clientWidth;
    //     let contentwidth = this.Tabs.length;
    //     for(var i=0;i<tabbox.children.length;i++){
    //         contentwidth+=tabbox.children[i].clientWidth
    //     }
    //     if(totalwidth<contentwidth){
    //         this.$store.commit("tab/DelTab", this.Tabs.length-2);
    //     }
    //   });
    // }
  },
  provide() {
    return {
      reload: this.reload
    };
  }
};
</script>

<style lang='scss' scoped>
.tabbody {
  height: 100%;
  overflow: hidden;
  display: flex;
  // flex-direction: column;
  flex-wrap: wrap;
}

.el-main {
  height: 100%;
  padding: 0;
  // margin: 10px;
  // box-shadow: 0px 0px 4px #999;
}
.scroll-container {
  width: 100%;
  height: auto;
  white-space: nowrap;
  /deep/ {
    .el-scrollbar__bar {
      bottom: 0px;
      &.is-vertical {
        display: none;
      }
    }
    .el-scrollbar__wrap {
      height: 53px;
    }
  }
}
.tab_box {
  background-color: #f1f1f1;
  &:after{
    content: '';
    display: inline-block;
    width: 100%;
    height: 1px;
    background-color: #ddd;
    position: absolute;
    bottom: 0;
    left: 0;
  }
  .el-tag {
    cursor: pointer;
    display: inline-block;
    height: 35px;
    line-height: 35px;
    border-right: solid 1px #ddd;
    border-top: solid 1px #ddd;
    border-bottom: none;
    border-left: none;
    color: #475059;
    background: #f1f1f1;
    padding: 0 10px;
    font-size: 12px;
    border-radius: 0;
    i {
      color: #495060;
      &:hover {
        background-color: #b4bccc;
        color: #fff;
      }
    }
  }
  .el-tag.active {
    background-color: #fff;
    color: #666;
    border-bottom-color: #fff;
    height: 36px;
    margin-bottom: -1px;
    position: relative;
    z-index: 1;
    i {
      color: #666;
    }
    // .tab_rect {
    //   content: "";
    //   background: #fff;
    //   display: inline-block;
    //   width: 8px;
    //   height: 8px;
    //   border-radius: 50%;
    //   position: relative;
    //   margin-right: 2px;
    // }
  }
}
</style>