<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <span>客户端名称:</span>
                <el-input v-model="keyword" size="mini" @keyup.enter.native="currentPage=1;flush();" placeholder="输入关键字搜索"/>
                <el-button type="primary" size="mini" @click='currentPage=1;flush();'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='edit(0)'>创建客户端</el-button>
                <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除客户端</el-button>
            </div>
            <el-table
                    ref="multipleTable"
                    :data="items"
                    tooltip-effect="dark"
                    border
                    align='center'
                    :stripe='true'
                    size='mini'
                    style="width: 100%"
                    @selection-change="handleSelectionChange">
                <el-table-column
                        type="selection"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        width="100"
                        prop="clientId"
                        label="客户端id"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="clientName"
                        label="客户端名称"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        width="200"
                        align='center'
                        label="授权类型">
                    <template slot-scope="scope">
                        <div style="text-align:left">
                            <span class="el-tag el-tag--info el-tag--medium"
                                v-for="p in scope.row.allowedGrantTypes"
                                :key="p">
                                {{p}} 
                            </span>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column
                        width="300"
                        align='center'
                        label="访问资源">
                    <template slot-scope="scope">
                        <div style="text-align:left">
                            <span class="el-tag el-tag--info el-tag--medium"
                                v-for="p in scope.row.allowedScopes"
                                :key="p">
                                {{p}} 
                            </span>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column
                        label="操作"
                        align='center'
                        width='240'
                >
                    <template slot-scope="scope">
                        <el-button type="primary" size="mini" @click='edit(1,scope.row)'>编辑</el-button>
                        <el-button type="primary" size="mini" @click='edit(2,scope.row)'>复制新增</el-button>
                        <!-- <el-button @click="roleCheck(scope.row)" type="text" size="small">模块管理</el-button> -->
                    </template>
                </el-table-column>
            </el-table>
            <el-pagination
                            class='page_footer_box'
                            @size-change="currentPage=1;flush();"
                            @current-change="flush"
                            :current-page.sync="currentPage"
                            :page-sizes="[10, 20, 30, 50]"
                            :page-size.sync="pageSize"
                            layout="total, sizes, prev, pager, next, jumper"
                            :total="total">
            </el-pagination>
            
        </div>
    </el-scrollbar>
</template>

<script>
    export default {
        components: {
        },
        data() {
            return {
                items: [],
                keyword: '',
                systemSelect: '',
                show: false,
                // 分页
                currentPage: 1,
                pageSize:10,
                total:0
            }
        },
        mounted() {
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/Client/Query", {"pageIndex": this.currentPage, "pageSize": this.pageSize,name:this.keyword});
                this.items = result.list;
                this.total = result.totalCount;
            },
            handleSelectionChange(val) {
                this.selectitems = val;
            },
            async delRole() {
                //this.$refs.multipleTable.clearSelection()
                //this.$refs.multipleTable.toggleRowSelection(row) this.$refs.multipleTable.selection
                if(this.selectitems&&this.selectitems.length){
                    await this.$confirm('确认删除所选客户端?', '删除', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    });
                    const codes = this.selectitems.map(item=>item.code);
                    await this.$http.post("/base/api/App/Delete",codes);
                    if(this.items.length==codes.length&&this.currentPage) this.currentPage--;//全部删除返回上一页
                    this.flush();
                    this.$message({
                        showClose: true,
                        type: 'success',
                        message: '删除成功!'
                    });
                }
                else{
                    this.$message({
                        showClose: true,
                        type: 'warning',
                        message: '请选择要删除的客户端'
                    });
                }
            },
            roleCheck(row) {
                console.log(row);
            },
            // 客户端编辑
            edit(type,row) {
                switch(type){
                    case 0:
                        this.$router.push("/home/client/add?id=0");break;
                    case 1:
                        this.$router.push("/home/client/update?id="+row.clientId);break;
                    case 2:
                        this.$router.push("/home/client/add?id="+row.clientId);break;
                }
            }
        }
    }
</script>

<style lang='scss' scoped>
    .el-tag{
       margin: 1px 3px;
       padding: 0 5px;
       .el-icon-close{
            width: 16px;
            right: 0;
       }
    }
    .el-tag:hover{
        cursor: pointer;
    }
    .page_footer_box {
        float: right;
        margin: 3px 10px;
    }
</style>