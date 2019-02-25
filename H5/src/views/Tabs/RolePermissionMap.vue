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
        <el-collapse-item title="一致性 Consistency" name="1">
            <template slot="title">
                <el-checkbox v-model="checked"></el-checkbox>一致性 Consistency
            </template>
            <div>
              <sapn>模块名称:</sapn>
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
    margin: 8px 15px;
    color: #666;
    /deep/ {
      .el-collapse-item__content{
          padding-bottom: 5px;
      }
  }
}

.el-checkbox{
    padding: 0 4px;
}
</style>