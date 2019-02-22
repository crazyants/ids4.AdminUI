<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <span>选择系统 :</span>
                <el-select size="mini" v-model='code' @change="currentPage=1;flush();" filterable placeholder="请选择系统">
                        <el-option label="全部" value=""></el-option>
                        <el-option v-for="app in apps" :key="app.code" :label="app.name" :value="app.code"></el-option>
                </el-select>
                <span>模块名称:</span>
                <el-input v-model="keyword" size="mini" @keyup.enter.native="currentPage=1;flush();" placeholder="输入关键字搜索"/>
                <el-button type="primary" size="mini" @click='currentPage=1;flush();'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='edit(0)'>创建模块</el-button>
                <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除模块</el-button>
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
                <!-- <el-table-column
                        width="80"
                        align='center'
                        label="序号">
                    <template slot-scope="scope">
                        {{(currentPage - 1 ) * pageSize + scope.$index  + 1}}
                    </template>
                </el-table-column> -->
                <el-table-column
                        prop="appName"
                        label="所属系统"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="code"
                        label="模块编码"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="name"
                        label="模块名称"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        align='center'
                        label="权限">
                    <template slot-scope="scope">
                        <span v-for="p in scope.row.permissions" :key="p.id" >{{p.name}}</span>
                    </template>
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
                        <el-button type="text" size="small" @click='addPermission(0,scope.row)'>添加权限</el-button>
                        <el-button @click="edit(1,scope.row)" type="text" size="small">修改模块</el-button>
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
        <ModuleEdit :config="config" @close="flush" ></ModuleEdit>
        <AddPermission :config="config2" @close="flush" ></AddPermission>
    </el-scrollbar>
</template>

<script>
    import ModuleEdit from '../../components/dialog/ModuleEdit'
    import AddPermission from '../../components/dialog/AddPermission'

    export default {

        components: {
            ModuleEdit,
            AddPermission
        },
        data() {
            return {
                apps:[],
                roleData: [],
                keyword: '',
                code: '',
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
                },
                config2:{
                    show:false,
                    width:500,
                    title:"",
                    data:{}
                }
            }
        },
        mounted() {
            this.$http.post("/base/api/App/Query", {"pageIndex": 1, "pageSize": 9999}).then(result=>{
                this.apps.push(...result.list);
            })
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/App/QueryModule", {code:this.code,pageIndex: this.currentPage, pageSize: this.pageSize,name:this.keyword});
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
                    await this.$confirm('确认删除所选模块?', '删除', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    });
                    const ids = this.selectitems.map(item=>item.id);
                    await this.$http.post("/base/api/App/DelModule",ids);
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
                        message: '请选择要删除的模块'
                    });
                }
            },
            addPermission(type,row) {
                this.config2.type = type;
                switch(type){
                    case 0:
                        this.config2.title=`添加权限 (${row.appName}-${row.name})`;this.config2.data = {moduleId:row.id};break;
                    case 1:
                        this.config2.title=`修改权限 (${row.appName}-${row.name})`;this.config2.data = {name:row.name,code:row.code,id:row.id};break;
                }
                this.config2.show = true;
            },
            edit(type,row) {
                this.config.type = type;
                switch(type){
                    case 0:
                        if(this.code) {this.config.title='创建模块';this.config.data = {appCode:this.code};break;}
                        else{
                            this.$message({ showClose: true, type: 'warning', message: '请选择系统,再创建模块' });
                        }
                    case 1:
                        this.config.title='编辑模块';this.config.data = {name:row.name,code:row.code,id:row.id};break;
                }
                this.config.show = true;
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