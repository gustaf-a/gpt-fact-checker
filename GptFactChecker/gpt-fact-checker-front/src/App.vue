<script setup lang="ts">
import { RouterView } from "vue-router";
import Nav from "./components/Header/Nav.vue";
import Footer from "@/components/Footer/Footer.vue"
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
