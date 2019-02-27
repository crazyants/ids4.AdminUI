<template>
  <el-scrollbar wrap-class="scrollbar-wrapper-y">
    <div>
      <div class="flex">
        <span>选择系统 :</span>
        <el-select
          size="mini"
          v-model="code"
          @change="currentPage=1;flush();"
          filterable
          placeholder="请选择系统"
        >
          <el-option label="全部" value></el-option>
          <el-option v-for="app in apps" :key="app.code" :label="app.name" :value="app.code"></el-option>
        </el-select>
        <div class="flex1"></div>
        <el-button type="danger" size="mini" @click="back">关闭并返回</el-button>
        <el-button type="success" size="mini" @click="save">保存</el-button>
      </div>
      <el-collapse v-model="activecodes">
        <el-collapse-item v-for="app in filterapps" :key="app.code" :name="app.code">
            <template slot="title">
                <el-checkbox :indeterminate="app.indeterminate" v-model="app.checked" @change="checkapp(app)"></el-checkbox> <span>{{app.name}}</span>
            </template>
            <div class="collapse-content" v-for="m in app.modules" :key="m.id">
              <el-checkbox :indeterminate="m.indeterminate" v-model="m.checked" @change="checkmodule(m,app)">{{m.name}}:</el-checkbox>
              <el-checkbox v-for="p in m.permissions" :key="p.id" v-model="p.checked" @change="checkmoduleAll(m,app)">{{p.name}}</el-checkbox>
            </div>
        </el-collapse-item>
      </el-collapse>
    </div>
  </el-scrollbar>
</template>

<script>

export default {
  components: {},
  data() {
    return {
      apps: [],
      code: "",
      activecodes: ["1"],
      keyword: "",
      systemSelect: ""
    };
  },
  async mounted() {
    const getapps = this.$http.get("/base/api/App/AppDeatil");
    const getcols = this.$http.get(`/base/api/Role/GetPermission?id=${this.$route.params.roleid}`);
    const apps = await getapps;
    const nowapps = await getcols;
    apps.forEach(app=>{
      app.checked = false;
      app.indeterminate = false;
      app.modules.forEach(m=>{
        m.indeterminate = false;
        m.checked = false;
        m.permissions.forEach(p=>{
          p.checked = false;
        })
      })
    })
    nowapps.forEach(nowapp=>{
      const app = apps.find(a=>a.code==nowapp.appCode);
      nowapp.modules.forEach(nowm=>{
        const m = app.modules.find(a=>a.id==nowm.moduleId);
        nowm.permissions.forEach(nowp=>{
          const p = m.permissions.find(a=>a.id==nowp.permissionId);
          p.checked = true;
        })
        m.checked = true;
        this.checkmoduleAll(m);
      })
      app.checked = true;
      this.checkappAll(app);
    })
    this.apps.push(...apps);
    this.activecodes = apps.map(item=>item.code);
  },
  computed:{
    filterapps(){
      if(this.code)
        return this.apps.filter(a=>a.code==this.code)
      else return this.apps;
    }
  },
  methods: {
    async flush() {
      // let result = await this.$http.post("/base/api/App/Query", {
      //   pageIndex: this.currentPage,
      //   pageSize: this.pageSize,
      //   name: this.keyword
      // });
      // this.items = result.list;
      // this.total = result.totalCount;
    },
    checkapp(app){
      app.indeterminate = false;
      app.modules.forEach(m=>{
        m.indeterminate = false;
        m.checked = app.checked;
        m.permissions.forEach(p=>{
          p.checked = app.checked;
        })
      })
    },
    checkmodule(m,app){
      m.indeterminate = false;
      if(m.checked) app.checked = m.checked;
      m.permissions.forEach(p=>{
        p.checked = m.checked;
      })
      this.checkappAll(app);
    },
    checkmoduleAll(m,app){
      if(!m.permissions.length) return;
      var mcheckcount = m.permissions.filter(a=>a.checked).length;
      if(mcheckcount){
        if(mcheckcount==m.permissions.length){
          m.indeterminate = false;
          m.checked = true;
        }
        else m.indeterminate = true;
      }
      else {
        m.indeterminate = false;
        m.checked = false;
      }
      if(app) this.checkappAll(app)
    },
    checkappAll(app){
      if(!app.modules.length) return;
      var checkcount = app.modules.filter(a=>a.checked&&!a.indeterminate).length;
      if(checkcount&&app.modules.length==checkcount){
        app.indeterminate = false;
        app.checked = true;
        return;
      }
      checkcount = app.modules.filter(a=>!a.checked&&!a.indeterminate).length;
      if(checkcount&&app.modules.length==checkcount){
        app.indeterminate = false;
        app.checked = false;
        return;
      }
      app.indeterminate = true;
    },
    async save(){
      var body = [];
      this.apps.forEach(app=>{
        if(app.checked||app.indeterminate){
          var newa = {appCode:app.code};
          newa.modules = [];
          app.modules.forEach(m=>{
            if(m.checked||m.indeterminate){
              var newm = {moduleId:m.id};
              newm.permissions = [];
              m.permissions.forEach(p=>{
                if(p.checked) newm.permissions.push({permissionId:p.id});
              })
              newa.modules.push(newm);
            }
          })
          body.push(newa);
        }
      })
      console.log(body);
      await this.$http.post(`/base/api/Role/SetPermission?id=${this.$route.params.roleid}`,body);
      this.$message({ showClose: true, type: 'success', message: '保存成功!' });
    },
    back(){
      this.$store.commit("tab/closeAndTo", {fullPath:"/home/role"});
    }
  }
};
</script>

<style lang='scss' scoped>
.page_footer_box {
  float: right;
  margin: 3px 10px;
}

.el-collapse{
    margin: 8px 30px;
    /deep/ {
      .el-collapse-item__content{
        padding-top: 0;
        padding-bottom: 10px;
      }
      .el-collapse-item__header{
        font-size: 12px;
        color: #606266;
      }
  }
}

.el-checkbox{
    padding: 0 4px;
    margin-right: 0;
}

.collapse-content{
    padding-left: 22px;
    margin-bottom: 4px;
    .el-checkbox{
        margin-left: 6px;
        /deep/ {
          .el-checkbox__label{
            padding-left: 3px;
            color: #606266;
            font-size: 12px;
          }
        }
    }
}

.flex>.el-button{
    margin: 0 8px;
}
.flex>.el-button:last-child{
    margin-right: 20px;
}
</style>