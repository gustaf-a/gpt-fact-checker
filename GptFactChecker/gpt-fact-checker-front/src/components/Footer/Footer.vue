<script setup lang="ts">
import ContainerVue from "../Container.vue";
</script>

<template>
	<a-layout-footer class="footer">
		<ContainerVue>
			<h3 class="footer-title">Fact Friend</h3>
			<a-divider class="divider" />
		</ContainerVue>
	</a-layout-footer>
</template>

<style scoped>
.footer {
	height: 30vh;
	background: #576f72;
	margin-top: 10vh;
}

.footer-title {
	font-size: large;
	color: white;
}

.divider {
	margin: 0;
	border-color: rgb(196, 196, 196);
}
</style>
