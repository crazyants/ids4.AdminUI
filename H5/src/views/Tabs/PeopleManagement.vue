<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <!-- <span>选择系统 :</span>
                <el-select size="mini" v-model='systemSelect' placeholder="请选择系统">
                        <el-option label="区域一" value="shanghai"></el-option>
                        <el-option label="区域二" value="beijing"></el-option>
                </el-select> -->
                <span>用户名称:</span>
                <el-input v-model="keyword" size="mini" @keyup.enter.native="currentPage=1;flush();" placeholder="输入关键字搜索"/>
                <el-button type="primary" size="mini" @click='currentPage=1;flush();'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='eduitNameRole()'>创建用户</el-button>
                <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除用户</el-button>
            </div>
            <el-table
                    ref="multipleTable"
                    :data="roleData"
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
                        width="80"
                        align='center'
                        label="序号">
                    <template slot-scope="scope">
                        {{(currentPage - 1 ) * pageSize + scope.$index  + 1}}
                    </template>
                </el-table-column>
                <el-table-column
                        prop="account"
                        label="账号"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="name"
                        label="用户名称"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="createTime"
                        label="创建时间"
                        align='center'
                        show-overflow-tooltip>
                </el-table-column>
                <el-table-column
                        label="操作"
                        align='center'
                        width='160'
                >
                    <template slot-scope="scope">
                        <el-button type="text" size="small" @click='eduitNameRole(scope.row)'>编辑</el-button>
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
        <PeopleEduit :config="config" @close="flush" ></PeopleEduit>
    </el-scrollbar>
</template>

<script>
    import PeopleEduit from '../../components/dialog/PeopleEduit'

    export default {

        components: {
           PeopleEduit
        },
        data() {
            return {
                roleData: [],
                keyword: '',
                systemSelect: '',
                show: false,
                // 分页
                currentPage: 1,
                pageSize:10,
                total:0,
                config:{
                    show:false,
                    width:500,
                    title:"",
                    isadd:false,
                    data:{}
                }
            }
        },
        mounted() {
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/User/Query", {"pageIndex": this.currentPage, "pageSize": this.pageSize,name:this.keyword});
                this.roleData = result.list;
                this.total = result.totalCount;
            },
            handleSelectionChange(val) {
                this.selectitems = val;
            },
            async delRole() {
                //this.$refs.multipleTable.clearSelection()
                //this.$refs.multipleTable.toggleRowSelection(row) this.$refs.multipleTable.selection
                if(this.selectitems&&this.selectitems.length){
                    await this.$confirm('确认删除所选用户?', '删除', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    });
                    const ids = this.selectitems.map(item=>item.id);
                    await this.$http.post("/base/api/Role/Delete",ids);
                    if(this.roleData.length==ids.length) this.currentPage--;//全部删除返回上一页
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
                        message: '请选择要删除的用户'
                    });
                }
            },
            // 用户名编辑
            eduitNameRole(row) {
                if(row){
                    this.config.isadd = false;
                    this.config.title='编辑用户名称';
                    this.config.data = Object.assign({},row);
                }else {
                    this.config.isadd = true;
                    this.config.title='创建用户名称';
                    this.config.data = {}
                }
                this.config.show = true
            }
        }
    }
</script>

<style lang='scss' scoped>
    .page_footer_box {
        float: right;
        margin: 3px 10px;
    }
</style>