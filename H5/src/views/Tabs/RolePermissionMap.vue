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
      </div>
      <el-collapse v-model="activecodes">
        <el-collapse-item v-for="app in apps" :key="app.code" title="一致性 Consistency" :name="app.code">
            <template slot="title">
                <el-checkbox v-model="app.checked"></el-checkbox>{{app.name}}
            </template>
            <div class="collapse-content">
              <span>模块名称:</span>
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
  mounted() {
    this.$http
      .post("/base/api/App/Query", { pageIndex: 1, pageSize: 9999 })
      .then(result => {
        this.apps.push(...result.list);
        this.activecodes = result.list.map(item=>item.code);
      });
    this.flush();
  },
  methods: {
    async flush() {
      let result = await this.$http.post("/base/api/App/Query", {
        pageIndex: this.currentPage,
        pageSize: this.pageSize,
        name: this.keyword
      });
      this.items = result.list;
      this.total = result.totalCount;
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
          padding: 5px 0;
      }
  }
}

.el-checkbox{
    padding: 0 4px;
}

.collapse-content{
    padding-left: 22px;
}
</style>