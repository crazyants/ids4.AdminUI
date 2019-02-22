<template>
  <div class="tabbody"  @dragover="allowDrop($event)" @drop="drop($event)">
    <el-scrollbar class="tab_box scroll-container" wrap-class="scrollbar-wrapper-y" ref="tabbox">
      <span draggable="true" @dragstart="drag($event,index)" 
        v-for="(tab,index) in Tabs"
        class="el-tag"
        :class="{'active':CurTabIndex==index}"
        @click="ActiveTab(index)"
        :key="tab.routername"
        @contextmenu.prevent="openMenu(index,$event);"
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
      <ul v-show="visible" :style="{left:left+'px',top:top+'px'}" class="contextmenu">
          <li @click="reflush">刷新(F5)</li>
          <li @click="DelTab(selectedTagIndex,$event)">关闭当前(Esc)</li>
          <li @click="DelRight(selectedTagIndex)">关闭右侧标签</li>
          <!-- <li @click="Fixed(selectedTagIndex);updatescroll();">固定/取消</li> -->
          <li @click="DelOther(selectedTagIndex)">关闭其他标签</li>
          <li @click="DelAll">全部关闭</li>
      </ul>
    <el-main>
      <keep-alive>
        <router-view v-if="isRouterAlive" :key="$route.fullPath" ref="cur"/>
      </keep-alive>
    </el-main>
  </div>
</template>

<script>
import { mapState, mapActions, mapMutations } from "vuex";

export default {
  name: "tab-view",
  data() {
    return {
        visible: false,
        top: 0,
        left: 0,
        selectedTagIndex: {},
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
    });
    document.addEventListener("keydown",this.keypress)
  },
  methods: {
    ...mapMutations("tab", ["ActiveTab","DelOther","DelAll","Fixed","DelRight"]),
    ...mapActions("tab", ["reflush"]),
    DelTab(index, e) {
      e.stopPropagation();
      this.$store.commit("tab/DelTab", index);
      this.visible = false
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
    keypress(e){
      switch (e.keyCode) {
        case 27:
          e.preventDefault();
          e.stopPropagation();
          if(this.CurTabIndex) this.$store.commit("tab/DelTab", this.CurTabIndex);
          break;
        case 116:
          e.preventDefault();
          e.stopPropagation();
          this.reflush();
          break;
        default:
          break;
      } 
      this.visible = false;
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
      const interval = this.$interval = setInterval(()=>{
        if (Math.abs(dv)>2) {
          let d = parseInt(dv/3);
          el.scrollLeft +=d;
          dv = dv - d;
        }
        else{
          el.scrollLeft += dv;
          clearInterval(interval);
        }
      },25);
    },
    updatescroll(){
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
    reload() {},

      openMenu(index, e) {
          const menuMinWidth = 105
          const offsetLeft = this.$el.getBoundingClientRect().left // container margin left
          const offsetWidth = this.$el.offsetWidth // container width
          const maxLeft = offsetWidth - menuMinWidth // left boundary
          const left = e.clientX - offsetLeft + 15 // 15: margin right

          if (left > maxLeft) {
              this.left = maxLeft
          } else {
              this.left = left
          }
          this.top = e.clientY - 50

          this.visible = true
          this.selectedTagIndex = index
      },
      closeMenu() {
          this.visible = false
      },
      refreshSelectedTag(view) {
          alert('刷新当前标签')
      },
      closeOthersTags() {
          alert('关闭其他标签')
      },
      closeAllTags() {
          alert('关闭所有标签')
      },
  },
  watch: {
    tabcount() {
      this.updatescroll();
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
        if(dis!=0) this.scrollDis(dis);
      });
    },
      visible(value) {
          if (value) {
              document.body.addEventListener('click', this.closeMenu)
          } else {
              document.body.removeEventListener('click', this.closeMenu)
          }
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
  beforeDestroy(){
    document.body.removeEventListener('click', this.closeMenu)
    document.removeEventListener("keydown",this.keypress)
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

  display: box;              /* OLD - Android 4.4- */
  display: -webkit-box;      /* OLD - iOS 6-, Safari 3.1-6 */
  display: -moz-box;         /* OLD - Firefox 19- (buggy but mostly works) */
  display: -ms-flexbox;      /* TWEENER - IE 10 */
  display: -webkit-flex;     /* NEW - Chrome */
  display: flex;             /* NEW, Spec - Opera 12.1, Firefox 20+ */
  
   /* 09版 */
  -webkit-box-orient: horizontal;
  /* 12版 */
  -webkit-flex-direction: column;
  -moz-flex-direction: column;
  -ms-flex-direction: column;
  -o-flex-direction: column;
  flex-direction: column;

  /* 09版 */
  /*-webkit-box-lines: multiple;*/
  /* 12版 */
  -webkit-flex-wrap: wrap;
  -moz-flex-wrap: wrap;
  -ms-flex-wrap: wrap;
  -o-flex-wrap: wrap;
  flex-wrap: wrap;
}

.el-main {
  -webkit-box-flex: 1;      /* OLD - iOS 6-, Safari 3.1-6 */
  -moz-box-flex: 1;         /* OLD - Firefox 19- */
  width: 20%;               /* For old syntax, otherwise collapses. */
  -webkit-flex: 1;          /* Chrome */
  -ms-flex: 1;              /* IE 10 */
  flex: 1;                  /* NEW, Spec - Opera 12.1, Firefox 20+ */

  width: 100%;
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
  background-color: #fafafa;
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
    background: #fafafa;
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
.contextmenu {
    margin: 0;
    background: #fff;
    z-index: 100;
    position: absolute;
    list-style-type: none;
    padding: 5px 0;
    // border-radius: 4px;
    font-size: 12px;
    font-weight: 400;
    color: #333;
    box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, .3);
    border-top: 1px solid rgba(0, 0, 0, .1);
    border-left: 1px solid rgba(0, 0, 0, .1);
    li {
        margin: 0;
        padding: 7px 16px;
        cursor: pointer;
        &:hover {
            background: #eee;
        }
    }
}
</style>