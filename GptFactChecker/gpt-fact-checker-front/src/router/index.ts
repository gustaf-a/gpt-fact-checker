import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";

import SourceView from "../views/SourceView.vue";
import SourcesViewVue from "@/views/SourcesView.vue";
import ResourcesViewVue from "@/views/ResourcesView.vue"
import ManageSourcesViewVue from "@/views/ManageSourcesView.vue";
import ManageUsersView from "@/views/ManageUsersView.vue";

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: "/",
			name: "home",
			component: HomeView,
		},
		{
			path: "/source/:sourceId",
			name: "source",
			component: SourceView,
		},
		{
			path: "/sources",
			name: "sources",
			component: SourcesViewVue,
		},
		{
			path: "/resources",
			name: "resources",
			component: ResourcesViewVue,
		},
		{
			path: "/manageusers",
			name: "manageusers",
			component: ManageUsersView,
		},
		{
			path: "/managesources",
			name: "managesources",
			component: ManageSourcesViewVue,
		},
		{
			path: "/about",
			name: "about",
			// route level code-splitting
			// this generates a separate chunk (About.[hash].js) for this route
			// which is lazy-loaded when the route is visited.
			component: () => import("../views/AboutView.vue"),
		},
	],
	scrollBehavior(to, from, savedPosition) {
		return { top: 0 }
	  }
});

export default router;
