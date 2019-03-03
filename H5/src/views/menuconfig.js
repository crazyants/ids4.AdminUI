//菜单配置
var menus = [
    {
        icon:"el-icon-menu",
        title:"权限管理",
        children:[
            {title:"系统配置",fullPath:'/home/app'},
            {title:"模块配置",fullPath:'/home/module'},
            // {title:"权限配置",fullPath:'/home/test'}
        ]
    },
    // {
    //     icon:"el-icon-menu",
    //     title:"系统配置",
    //     fullPath:'/home/app'
    // },
    {
        icon:"icon-id navicon icon-role",
        title:"角色管理",
        fullPath:'/home/role'
    },
    {
        icon:"icon-id navicon icon-user",
        title:"人员管理",
        fullPath:'/home/people'
    },
    {
        icon:"icon-id navicon icon-kehu",
        title:"客户端管理",
        fullPath:'/home/client'
    },
    {
        icon:"icon-id navicon icon-resource_icon",
        title:"资源管理",
        fullPath:'/home/apiresource'
    }
];

menus.forEach((menu1,index1)=>{
    menu1.index=index1.toString();
    if(menu1.children){
        menu1.children.forEach((menu2,index2)=>{
            menu2.index=index1+'.'+index2;
            if(menu2.children){
                menu2.children.forEach((menu3,index3)=>{
                    menu3.index=index1+'.'+index2+'.'+index3;
                })
            }
        })
    }
})


export default menus