<script setup lang="ts">
import { RouterView } from "vue-router";
import Nav from "./components/Header/Nav.vue";
import Footer from "@/components/Footer/Footer.vue"
import { onMounted } from "vue";
import { useUserStore } from "./stores/users";

const userStore = useUserStore();

onMounted(() => {
	userStore.getExistingUser();
});

</script>

<template>
	<main>
		<Nav />
		<RouterView />
		<Footer/>
	</main>
</template>

<style scoped>

</style>
