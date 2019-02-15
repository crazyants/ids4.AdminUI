<template>
  <div class="tabbody">
    <el-scrollbar class="tab_box scroll-container" ref="tabbox" @dragover="allowDrop($event)" @drop="drop($event)">
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
  methods: {
    ...mapMutations("tab", ["ActiveTab"]),
    DelTab(index, e) {
      e.stopPropagation();
      this.$store.commit("tab/DelTab", index);
    },
    drag(event,dragindex) {
       const dom = event.currentTarget;
       this.$start_x = event.clientX;
       this.$dragindex = dragindex;
    },
    drop(event) {
        const dx = event.clientX - this.$start_x;
        console.log(dx)
        event.preventDefault();
        const afterindex = this.getafterindex(dx);
        if(afterindex != this.$dragindex){
            this.$store.commit("tab/exchange", {before:this.$dragindex,after:afterindex});
        }
    },
    getafterindex(dx){//根据拖拽位移获取 移动后的index
        const tabbox = this.$refs.tabbox;
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
    },
    reload() {}
  },
  watch: {
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
  flex-direction: column;
}

.el-main {
  height: 100%;
  padding: 0;
  margin: 10px;
  box-shadow: 0px 0px 4px #999;
}
.scroll-container {
  white-space: nowrap;
  position: relative;
  overflow: hidden;
  width: 100%;
  height: auto;
  /deep/ {
    .el-scrollbar__bar {
      bottom: 0px;
    }
    .el-scrollbar__wrap {
      height: 49px;
    }
  }
}
.tab_box {
  border-bottom: 1px solid #ddd;
  background-color: #f1f1f1;
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