<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <!-- <span>选择系统 :</span>
                <el-select size="mini" v-model='systemSelect' placeholder="请选择系统">
                        <el-option label="区域一" value="shanghai"></el-option>
                        <el-option label="区域二" value="beijing"></el-option>
                </el-select> -->
                <span>角色名称:</span>
                <el-input v-model="keyword" size="mini" @keyup.enter.native="currentPage=1;flush();" placeholder="输入关键字搜索"/>
                <el-button type="primary" size="mini" @click='currentPage=1;flush();'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='eduitNameRole("")'>创建角色</el-button>
                <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除角色</el-button>
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
                        prop="id"
                        label="权限ID"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="name"
                        label="角色名称"
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
                        width='200'
                >
                    <template slot-scope="scope">
                        <el-button type="primary" size="mini" @click='eduitNameRole(scope.row)'>编辑</el-button>
                        <el-button @click="roleCheck(scope.row)" type="primary" size="mini">权限分配</el-button>
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
        <RoleNameEduit :config="config" @close="flush" ></RoleNameEduit>
    </el-scrollbar>
</template>

<script>
    import RoleNameEduit from '../../components/dialog/RoleNameEduit'

    export default {

        components: {
            RoleNameEduit,
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
                    data:{}
                }
            }
        },
        mounted() {
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/Role/Query", {"pageIndex": this.currentPage, "pageSize": this.pageSize,name:this.keyword});
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
                    await this.$confirm('确认删除所选角色?', '删除', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    });
                    const ids = this.selectitems.map(item=>item.id);
                    await this.$http.post("/base/api/Role/Delete",ids);
                    if(this.roleData.length==ids.length&&this.currentPage) this.currentPage--;//全部删除返回上一页
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
                        message: '请选择要删除的角色'
                    });
                }
            },
            roleCheck(row) {
                console.log(row);
            },
            // 角色名编辑
            eduitNameRole(row) {
                if(row){
                    this.config.title='编辑角色名称';
                    this.config.data = Object.assign({},row);
                }else {
                    this.config.title='创建角色名称';
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